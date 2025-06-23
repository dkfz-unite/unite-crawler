# UNITE Data Crawler and Readers
CLI applications for crawling and reading OMICS data from the file system.


## Data Structure
Data is organized in `donor` **per** `analysis` way, where each `donor` can have multiple `samples` and each `sample` can have multiple `processors` with resulting files.  

Data should be organized in the following structure:
```txt
<project>
└── omics
    ├── sampes.tsv # Samples sheet
    ├── ...
    └── <analysis>
        └── <donor> # Pseudonymised donor identifier (PID)
            └── <sample>
                ├── <sample_file>
                ├── ...
                └── <type[-vs_<sample>][-<format>]>
                    ├── <results_file>
                    └── ...
```

- `samples.tsv` - Samples sheet file with the metadata of the project samples.  
- `<analysis>` - Directory with the analysis data of specific analysis type. Named with the analysis type, _unique per `project`_ (e.g. `WGS`, `WES`, etc.).
- `<donor>` - Directory with the analysis data of the donor. Named with the pseudonymised donor identifier (PID), _unique per `project`_.
- `<sample>` - Directory with the sample data of the donor. Named with the sample name, _unique per `donor`_ (e.g. `tumor`, `blood`, etc.).
- `<type[-vs_<sample>][-<format>]` - Directory with the processed data of specific type. Named with type of the data (e.g. `cnv`, `exp`, etc.).
  - `-vs_<sample>` - Optional suffix to specify which sample was used for comparison during the data processing (e.g. `-vs_blood`).
  - `-<format>` - Optional suffix to specify which processing pipeline was used to generate the results (e.g. `-aseseq` - ACESeq CNV caller).  
  It defines which format has the data. If no format is specified, data in the default UNITE format will be expected for the type.


## Supported Data
The following types of the analyses, samples and other data is supported.

### DNA Sequencing
DNA sequencing data.

#### Analyses
- `WGS` - Whole genome sequencing.
- `WES` - Whole exome sequencing.

#### Samples
- `*.fasta*` - FASTA files with sequences.
- `*.fastq*` - FASTQ files with sequences.
- `*.bam`, `*.bam.bai`, `*.bam.bai.md5` - BAM files with aligned sequences and their indexes.

#### Data Types and Files
- `snv` - Single nucleotide variants calling.
  - `variants.tsv`
  - `variants.vcf`

- `indel` - Insertion/deletion variants calling.
  - `variants.tsv`
  - `variants.vcf`

- `sm` - Simple mutations calling (combined SNV and Indel).
  - `variants.tsv`
  - `variants.vcf`

- `cnv` - Copy number variants calling.
  - `variants[-<ploidy>-<purity>].tsv`
  - `variants[-<ploidy>-<purity>].vcf`  
  Suffix `[-<ploidy>-<purity>]` can be used to specify the **ploidy** and **purity** of the sample.  
  For compatibility with the file system, decimal numbers should use `_` instead of `.` (e.g. `2_0` for `2.0`).  
  If the suffix is not specified, default ploidy and purity values will be used (`2.0` and `null` respectively).

- `sv` - Structural variants calling.
  - `variants.tsv`
  - `variants.vcf` 

#### Example
```txt
omics
└── WGS
    └── PID0001
        ├── blood
        │   ├── sample.fastq.gz
        │   ├── sample.bam
        │   ├── sample.bam.bai
        │   └── sample.bam.bai.md5
        └── tumor
            ├── sample.fastq.gz
            ├── sample.bam
            ├── sample.bam.bai
            ├── sample.bam.bai.md5
            ├── snv-vs_boold
            │   └── variants.vcf
            ├── indel-vs_blood
            │   └── variants.vcf
            ├── cnv-vs_blood-aceseq
            │   └── variants-2_0-0_9.tsv
            └── sv-vs_blood-sophia
                └── variants.tsv
```

### Methylation Array Assay
Methylation array assay data.

#### Analyses
- `MethArray` - Illumina methylation array assay.

#### Samples
- `*red.idat` - IDAT files with methylation data for Red channel.
- `*grn.idat` - IDAT files with methylation data for Green channel.  
Both channels of the `*.idat` files should be present to be located and used.

#### Example
```txt
omics
└── MethArray
    └── PID0001
        └── tumor
            ├── sample_red.idat
            └── sample_grn.idat
```

### Bulk RNA Sequencing
Bulk RNA sequencing data.

#### Analyses
- `RNASeq` - Bulk RNA sequencing.

#### Samples
- `*.fasta*` - FASTA files with sequences.
- `*.fastq*` - FASTQ files with sequences.
- `*.bam`, `*.bam.bai`, `*.bam.bai.md5` - BAM files with aligned sequences and their indexes.

#### Data Types and Files
- `exp` - Expression quantification.
  - `levels.tsv` - file with expression levels.

#### Example
```txt
omics
└── RNASeq
    └── PID0001
        └── tumor
            ├── sample.fastq.gz
            ├── sample.bam
            ├── sample.bam.bai
            ├── sample.bam.bai.md5
            └── exp
                └── levels.tsv
```

### Single Cell/Nuclei RNA Sequencing
#### Analyses
- `scRNASeq` - Single cell RNA sequencing.
- `snRNASeq` - Single nuclei RNA sequencing.

#### Samples
- `*.fasta*` - FASTA files with sequences.
- `*.fastq*` - FASTQ files with sequences.
- `*.bam`, `*.bam.bai`, `*.bam.bai.md5` - BAM files with aligned sequences and their indexes.

#### Data Types and Files
- `exp` - Expression quantification (10x Genomics by default).
  - `barcodes.tsv.gz` - file with barcodes.
  - `features.tsv.gz` - file with features (genes).
  - `matrix.mtx.gz` - file with expression levels matrix.  
    All three files are required to be present in the `exp` directory to be located and used.

#### Example
```txt
omics
└── scRNASeq
    └── PID0001
        └── tumor
            ├── sample.fastq.gz
            ├── sample.bam
            ├── sample.bam.bai
            ├── sample.bam.bai.md5
            └── exp
                ├── barcodes.tsv.gz
                ├── features.tsv.gz
                └── matrix.mtx.gz
```


## Samples Sheet
Samples sheet (`samples.tsv`) is a file with the metadata of the project related samples.

The file should have the following columns:
- `donor_key`* - Donor identifier (as in the file system).
- `sample_key`* - Sample identifier (as in the file system).
- `donor_id` - Pseudonymised donor identifier (PID). Set to `donor_key` value by default. 
- `specimen_id` - Biological specimen identifier. Set to `sample_key` value by default. 
- `specimen_type` - Type of the biological specimen. Set to `Material` by default.
- `analysis_type` - Type of the analysis (if not specified, will be set from the files structure).
- `analysis_date` - Date of the analysis in ISO format `yyyy-mm-dd` (if not specified will be set from the file creation date).
- `analysis_day` - Day of the analysis relative to the donor enrollment (if exact date is not available).
- `genome` - Reference genome (`GRCh37` or `GRCh38`) used for the analysis (Defaults to `GRCh37`).

### Example
```tsv
donor_key sample_key  donor_id	specimen_id	specimen_type	analysis_type	analysis_date	analysis_day	genome
PID0001 Blood     Material	WGS	2023-01-01		GRCh38
PID0001 Tumor     Material	WGS	2023-01-01		GRCh38
PID0002 Bood      Material	WGS	2023-01-02		GRCh38
PID0002 Tumor     Material	WGS	2023-01-02		GRCh38
```
