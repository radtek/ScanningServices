using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ScanningServices.MSSQLEntities
{
    public partial class ScanningDBContext : DbContext
    {
        public virtual DbSet<BatchControl> BatchControl { get; set; }
        public virtual DbSet<BatchDocs> BatchDocs { get; set; }
        public virtual DbSet<BatchImages> BatchImages { get; set; }
        public virtual DbSet<BatchTracking> BatchTracking { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<ExportRules> ExportRules { get; set; }
        public virtual DbSet<GeneralSettings> GeneralSettings { get; set; }
        public virtual DbSet<JobPageSizes> JobPageSizes { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<JobsFields> JobsFields { get; set; }
        public virtual DbSet<JobsPostValidation> JobsPostValidation { get; set; }
        public virtual DbSet<JobsProcessesRemove> JobsProcessesRemove { get; set; }
        public virtual DbSet<JobsValidationRoutines> JobsValidationRoutines { get; set; }
        public virtual DbSet<PdfstationsRemove> PdfstationsRemove { get; set; }
        public virtual DbSet<Processes> Processes { get; set; }
        public virtual DbSet<ProcessesTypes> ProcessesTypes { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ReportParameters> ReportParameters { get; set; }
        public virtual DbSet<Reports> Reports { get; set; }
        public virtual DbSet<ReportsTemplate> ReportsTemplate { get; set; }
        public virtual DbSet<ReportsTemplateParameters> ReportsTemplateParameters { get; set; }
        public virtual DbSet<ServiceStations> ServiceStations { get; set; }
        public virtual DbSet<Smtp> Smtp { get; set; }
        public virtual DbSet<Sssfunctionality> Sssfunctionality { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserUifunctionality> UserUifunctionality { get; set; }
        public virtual DbSet<Vfr> Vfr { get; set; }
        public virtual DbSet<Vfrdocs> Vfrdocs { get; set; }
        public virtual DbSet<WorkingFolders> WorkingFolders { get; set; }

        // Unable to generate entity type for table 'dbo.BatchMapping'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.FieldNames'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.FieldValues'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SCANNING_SERVICES_DB'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BatchStatusEntity'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer(@"Server=cdlalo4\SQLExpress;Database=SCANNING_SERVICES;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer(ScanningServicesDataObjects.GlobalVars.connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatchControl>(entity =>
            {
                entity.HasKey(e => e.BatchNumber);

                entity.Property(e => e.BatchNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.BatchAlias).HasMaxLength(50);

                entity.Property(e => e.BatchSize).HasDefaultValueSql("((0))");

                entity.Property(e => e.BlockNumber).HasDefaultValueSql("((0))");

                entity.Property(e => e.CaptureTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.Customer).HasMaxLength(50);

                entity.Property(e => e.DeptName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentPath).HasColumnType("text");

                entity.Property(e => e.ExportedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExportedDate).HasColumnType("datetime");

                entity.Property(e => e.ExportedTimes).HasDefaultValueSql("((0))");

                entity.Property(e => e.FileStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KodakErrorState).HasMaxLength(100);

                entity.Property(e => e.KodakStatus).HasMaxLength(50);

                entity.Property(e => e.LastTimeRejected).HasColumnType("datetime");

                entity.Property(e => e.LotNumber).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedStation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfScannedPages).HasDefaultValueSql("((0))");

                entity.Property(e => e.OutputBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OutputDate).HasColumnType("datetime");

                entity.Property(e => e.OutputStation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PrepDate).HasColumnType("datetime");

                entity.Property(e => e.PrepUserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Qarflag)
                    .HasColumnName("QARFlag")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Qcby)
                    .HasColumnName("QCBy")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Qcdate)
                    .HasColumnName("QCDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.QcendTime)
                    .HasColumnName("QCEndTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.QcstageTime).HasColumnName("QCStageTime");

                entity.Property(e => e.QcstartTime)
                    .HasColumnName("QCStartTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Qcstation)
                    .HasColumnName("QCStation")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qctime).HasColumnName("QCTime");

                entity.Property(e => e.RecallBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecallDate).HasColumnType("smalldatetime");

                entity.Property(e => e.RecallReason).HasColumnType("text");

                entity.Property(e => e.RejectedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RejectedTimes).HasDefaultValueSql("((0))");

                entity.Property(e => e.RejectionReason).HasColumnType("text");

                entity.Property(e => e.ScanOperator)
                    .HasColumnName("ScanOPerator")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScanStation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ScannedDate).HasColumnType("datetime");

                entity.Property(e => e.ScanningEndTime).HasColumnType("smalldatetime");

                entity.Property(e => e.StatusFlag)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubDeptName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubmittedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubmittedDate).HasColumnType("datetime");

                entity.Property(e => e.TaskOrder)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VfrstatusFlag)
                    .HasColumnName("VFRStatusFlag")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VfruploadCompletedDate)
                    .HasColumnName("VFRUploadCompletedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.VfruploadDate)
                    .HasColumnName("VFRUploadDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.VfruploadModifiedDate)
                    .HasColumnName("VFRUploadModifiedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.VfruploadRequestedDate)
                    .HasColumnName("VFRUploadRequestedDate")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<BatchDocs>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.BatchName });

                entity.Property(e => e.DocumentId)
                    .HasColumnName("Document ID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BatchName)
                    .HasColumnName("Batch name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BatchLocation)
                    .IsRequired()
                    .HasColumnName("Batch Location")
                    .HasColumnType("text");

                entity.Property(e => e.BatchSize).HasColumnName("Batch size");

                entity.Property(e => e.BlackAndWhiteImageCount).HasColumnName("Black and white image count");

                entity.Property(e => e.ColorImageCount).HasColumnName("Color image count");

                entity.Property(e => e.CreatedDateAndTime)
                    .HasColumnName("Created date and time")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedStationId)
                    .IsRequired()
                    .HasColumnName("Created station ID")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedStationName)
                    .IsRequired()
                    .HasColumnName("Created station Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("Created user ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Customer).HasMaxLength(100);

                entity.Property(e => e.CustomerField1).HasColumnName("Customer Field 1");

                entity.Property(e => e.CustomerField10).HasColumnName("Customer Field 10");

                entity.Property(e => e.CustomerField11).HasColumnName("Customer Field 11");

                entity.Property(e => e.CustomerField12).HasColumnName("Customer Field 12");

                entity.Property(e => e.CustomerField13).HasColumnName("Customer Field 13");

                entity.Property(e => e.CustomerField14).HasColumnName("Customer Field 14");

                entity.Property(e => e.CustomerField15).HasColumnName("Customer Field 15");

                entity.Property(e => e.CustomerField2).HasColumnName("Customer Field 2");

                entity.Property(e => e.CustomerField3).HasColumnName("Customer Field 3");

                entity.Property(e => e.CustomerField4).HasColumnName("Customer Field 4");

                entity.Property(e => e.CustomerField5).HasColumnName("Customer Field 5");

                entity.Property(e => e.CustomerField6).HasColumnName("Customer Field 6");

                entity.Property(e => e.CustomerField7).HasColumnName("Customer Field 7");

                entity.Property(e => e.CustomerField8).HasColumnName("Customer Field 8");

                entity.Property(e => e.CustomerField9).HasColumnName("Customer Field 9");

                entity.Property(e => e.DocumentCountInBatch).HasColumnName("Document count in batch");

                entity.Property(e => e.DocumentFilename)
                    .IsRequired()
                    .HasColumnName("Document Filename")
                    .HasColumnType("text");

                entity.Property(e => e.DocumentFilenameWithFullPath)
                    .IsRequired()
                    .HasColumnName("Document Filename with Full Path")
                    .HasColumnType("text");

                entity.Property(e => e.DocumentLocation)
                    .IsRequired()
                    .HasColumnName("Document location")
                    .HasColumnType("text");

                entity.Property(e => e.DocumentSequenceNumber)
                    .IsRequired()
                    .HasColumnName("Document Sequence Number")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSize)
                    .IsRequired()
                    .HasColumnName("Document size")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstDocumentId)
                    .IsRequired()
                    .HasColumnName("First document ID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GrayscaleImageCount).HasColumnName("Grayscale image count");

                entity.Property(e => e.ImageCountInBatch).HasColumnName("Image count in batch");

                entity.Property(e => e.ImageCountInDocument).HasColumnName("Image count in document");

                entity.Property(e => e.ImageDeletedBack).HasColumnName("Image deleted - back");

                entity.Property(e => e.ImageDeletedFront).HasColumnName("Image deleted - front");

                entity.Property(e => e.ImagesCapturedBack).HasColumnName("Images captured - back");

                entity.Property(e => e.ImagesCapturedFront).HasColumnName("Images captured - front");

                entity.Property(e => e.ImagesRemovedForBlankBack).HasColumnName("Images removed for blank - back");

                entity.Property(e => e.ImagesRemovedForBlankFront).HasColumnName("Images removed for blank - front");

                entity.Property(e => e.ImagesRescannedBack).HasColumnName("Images rescanned - back");

                entity.Property(e => e.ImagesRescannedFront).HasColumnName("Images rescanned - front");

                entity.Property(e => e.Keystrokes).HasColumnName("keystrokes");

                entity.Property(e => e.LastDocumentId)
                    .IsRequired()
                    .HasColumnName("Last document ID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDateAndTime)
                    .HasColumnName("Last modified date and time")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastModifiedStationId)
                    .IsRequired()
                    .HasColumnName("Last modified station ID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedStationName)
                    .IsRequired()
                    .HasColumnName("Last modified station Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedUserId)
                    .IsRequired()
                    .HasColumnName("Last modified user ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OutputDateAndTime)
                    .HasColumnName("Output date and time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OutputStationId)
                    .IsRequired()
                    .HasColumnName("Output station ID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.OutputStationName)
                    .IsRequired()
                    .HasColumnName("Output station Name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OutputUserId)
                    .IsRequired()
                    .HasColumnName("Output user ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PageCountInBatch).HasColumnName("Page count in batch");

                entity.Property(e => e.PageCountInDocument).HasColumnName("Page count in document");

                entity.Property(e => e.StartingDocumentId)
                    .IsRequired()
                    .HasColumnName("Starting document ID")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BatchImages>(entity =>
            {
                entity.HasKey(e => new { e.BatchName, e.DocumentId, e.ImageSequenceNumber });

                entity.Property(e => e.BatchName)
                    .HasColumnName("Batch name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentId)
                    .HasColumnName("Document ID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ImageSequenceNumber)
                    .HasColumnName("Image Sequence Number")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ImageFilename)
                    .IsRequired()
                    .HasColumnName("Image Filename")
                    .HasColumnType("text");

                entity.Property(e => e.ImageFilenameWithFullPath)
                    .IsRequired()
                    .HasColumnName("Image Filename with Full Path")
                    .HasColumnType("text");

                entity.Property(e => e.ImageLocation)
                    .IsRequired()
                    .HasColumnName("Image location")
                    .HasColumnType("text");

                entity.Property(e => e.PageId)
                    .IsRequired()
                    .HasColumnName("Page ID")
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BatchTracking>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BatchNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Event)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FinalStatus).HasMaxLength(50);

                entity.Property(e => e.InitialStatus).HasMaxLength(50);

                entity.Property(e => e.OperatorId)
                    .HasColumnName("OperatorID")
                    .HasMaxLength(50);

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<ExportRules>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.Property(e => e.JobId)
                    .HasColumnName("JobID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DirectoryFormat).HasColumnType("ntext");

                entity.Property(e => e.DirectoryReplaceRule).HasColumnType("ntext");

                entity.Property(e => e.FieldsReplaceRule).HasColumnType("ntext");

                entity.Property(e => e.FileNameFormat).HasColumnType("ntext");

                entity.Property(e => e.FileNameReplaceRule).HasColumnType("ntext");

                entity.Property(e => e.IncludeHeaderFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.MetadataFileName).HasColumnType("nchar(30)");

                entity.Property(e => e.MetadataOutputDirectoryFormat).HasColumnType("ntext");

                entity.Property(e => e.OutputFields).HasColumnType("ntext");

                entity.Property(e => e.OutputFileDelimeter).HasColumnType("nchar(5)");

                entity.Property(e => e.OutputFileFormat).HasColumnType("nchar(30)");

                entity.Property(e => e.UseQuotationAroundFlag).HasColumnType("nchar(5)");
            });

            modelBuilder.Entity<GeneralSettings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AutoImportWatchFolderId).HasColumnName("AutoImportWatchFolderID");

                entity.Property(e => e.BackupFolderId).HasColumnName("BackupFolderID");

                entity.Property(e => e.BatchDeliveryWatchFolderId).HasColumnName("BatchDeliveryWatchFolderID");

                entity.Property(e => e.CpapplicationFilePath).HasColumnName("CPApplicationFilePath");

                entity.Property(e => e.Dbname)
                    .HasColumnName("DBName")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Dbpassword)
                    .HasColumnName("DBPassword")
                    .HasColumnType("nchar(30)");

                entity.Property(e => e.Dbprovider)
                    .HasColumnName("DBProvider")
                    .HasColumnType("nchar(30)");

                entity.Property(e => e.Dbrdbms)
                    .HasColumnName("DBRDBMS")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.Dbserver)
                    .HasColumnName("DBServer")
                    .HasColumnType("nchar(30)");

                entity.Property(e => e.DbuserName)
                    .HasColumnName("DBUserName")
                    .HasColumnType("nchar(30)");

                entity.Property(e => e.DebugFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.FileConversionWatchFolderId).HasColumnName("FileConversionWatchFolderID");

                entity.Property(e => e.LoadBalancerWatchFolderId).HasColumnName("LoadBalancerWatchFolderID");

                entity.Property(e => e.PostValidationWatchFolderId).HasColumnName("PostValidationWatchFolderID");

                entity.Property(e => e.QcoutputFolderId).HasColumnName("QCOutputFolderID");

                entity.Property(e => e.RestingLocationId).HasColumnName("RestingLocationID");

                entity.Property(e => e.ScanningFolderId).HasColumnName("ScanningFolderID");

                entity.Property(e => e.VfrbatchMonitorFolderId).HasColumnName("VFRBatchMonitorFolderID");

                entity.Property(e => e.VfrbatchUploaderWatchFolderId).HasColumnName("VFRBatchUploaderWatchFolderID");

                entity.Property(e => e.VfrduplicateRemoverWatchFolderId).HasColumnName("VFRDuplicateRemoverWatchFolderID");

                entity.Property(e => e.VfrrenamerWatchFolderId).HasColumnName("VFRRenamerWatchFolderID");
            });

            modelBuilder.Entity<JobPageSizes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnType("nchar(30)");

                entity.Property(e => e.JobId).HasColumnName("JobID");
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.AutoImportEnableFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.AutoImportWatchFolderId).HasColumnName("AutoImportWatchFolderID");

                entity.Property(e => e.BackupFolderId).HasColumnName("BackupFolderID");

                entity.Property(e => e.BatchCleanupFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.BatchDeliveryWatchFolderId).HasColumnName("BatchDeliveryWatchFolderID");

                entity.Property(e => e.DeleteBatchDeliveryEnableFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.DeleteLoadBalancerEnableFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.DepartmentName).HasColumnType("nchar(100)");

                entity.Property(e => e.ExportClassName).HasColumnType("nchar(100)");

                entity.Property(e => e.FileConversionFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.FileConversionWatchFolderId).HasColumnName("FileConversionWatchFolderID");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnType("nchar(100)");

                entity.Property(e => e.LoadBalancerWatchFolderId).HasColumnName("LoadBalancerWatchFolderID");

                entity.Property(e => e.MultiPageFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.OutputFileType).HasColumnType("nchar(20)");

                entity.Property(e => e.PostValidationEnableFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.PostValidationWatchFolderId).HasColumnName("PostValidationWatchFolderID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.QcoutputFolderId).HasColumnName("QCOutputFolderID");

                entity.Property(e => e.RestingLocationId).HasColumnName("RestingLocationID");

                entity.Property(e => e.ScanningFolderId).HasColumnName("ScanningFolderID");

                entity.Property(e => e.VfrbatchMonitorFolderId).HasColumnName("VFRBatchMonitorFolderID");

                entity.Property(e => e.VfrbatchUploaderWatchFolderId).HasColumnName("VFRBatchUploaderWatchFolderID");

                entity.Property(e => e.VfrduplicateRemoverWatchFolderId).HasColumnName("VFRDuplicateRemoverWatchFolderID");

                entity.Property(e => e.VfrenableFlag)
                    .HasColumnName("VFREnableFlag")
                    .HasColumnType("nchar(5)");

                entity.Property(e => e.VfrrenamerWatchFolderId).HasColumnName("VFRRenamerWatchFolderID");
            });

            modelBuilder.Entity<JobsFields>(entity =>
            {
                entity.HasKey(e => e.FieldId);

                entity.Property(e => e.FieldId).HasColumnName("FieldID");

                entity.Property(e => e.CpfieldName)
                    .HasColumnName("CPFieldName")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.KeyStrokeExcludeFlag)
                    .HasColumnType("char(5)")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.VfrfieldName)
                    .HasColumnName("VFRFieldName")
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<JobsPostValidation>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.Property(e => e.JobId)
                    .HasColumnName("JobID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EnableFlag)
                    .HasColumnType("nchar(5)")
                    .HasDefaultValueSql("(N'true')");

                entity.Property(e => e.LogFileName).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<JobsProcessesRemove>(entity =>
            {
                entity.HasKey(e => new { e.ProcessId, e.JobId });

                entity.ToTable("JobsProcesses.Remove");

                entity.Property(e => e.ProcessId).HasColumnName("ProcessID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.BackupFolderId).HasColumnName("BackupFolderID");

                entity.Property(e => e.Description).HasColumnType("nchar(100)");

                entity.Property(e => e.EnableFlag)
                    .HasColumnType("nchar(5)")
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Schedule).HasColumnType("ntext");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.SupersedeFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.TargetFolderId).HasColumnName("TargetFolderID");

                entity.Property(e => e.WatchFolderId).HasColumnName("WatchFolderID");
            });

            modelBuilder.Entity<JobsValidationRoutines>(entity =>
            {
                entity.HasKey(e => e.RoutineId);

                entity.Property(e => e.RoutineId).HasColumnName("RoutineID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.OperatorType)
                    .IsRequired()
                    .HasColumnType("nchar(10)")
                    .HasDefaultValueSql("(N'AND')");

                entity.Property(e => e.RoutineName).HasColumnType("nchar(100)");

                entity.Property(e => e.RoutineType).HasColumnType("nchar(50)");

                entity.Property(e => e.ValidationLevel).HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<PdfstationsRemove>(entity =>
            {
                entity.HasKey(e => e.PdfstationId);

                entity.ToTable("PDFStations.Remove");

                entity.Property(e => e.PdfstationId)
                    .HasColumnName("PDFStationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BackupFolderId).HasColumnName("BackupFolderID");

                entity.Property(e => e.EnableFlag)
                    .HasColumnName("EnableFLag")
                    .HasColumnType("nchar(5)");

                entity.Property(e => e.TargetFolderId).HasColumnName("TargetFolderID");

                entity.Property(e => e.WatchFolderId).HasColumnName("WatchFolderID");

                entity.Property(e => e.WeekDayFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.WeekendFlag).HasColumnType("nchar(5)");
            });

            modelBuilder.Entity<Processes>(entity =>
            {
                entity.HasKey(e => new { e.ProcessId, e.JobId, e.StationId, e.PdfstationId });

                entity.Property(e => e.ProcessId).HasColumnName("ProcessID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.PdfstationId).HasColumnName("PDFStationID");

                entity.Property(e => e.EnableFlag)
                    .IsRequired()
                    .HasColumnType("nchar(5)")
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.Schedule).HasColumnType("ntext");
            });

            modelBuilder.Entity<ProcessesTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<ReportParameters>(entity =>
            {
                entity.HasKey(e => new { e.ReportId, e.TemplateId, e.ParameterId });

                entity.Property(e => e.ReportId).HasColumnName("ReportID");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Reports>(entity =>
            {
                entity.HasKey(e => e.ReportId);

                entity.Property(e => e.ReportId).HasColumnName("ReportID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.EmailSubject).HasColumnType("nchar(150)");

                entity.Property(e => e.EnableFlag)
                    .IsRequired()
                    .HasColumnType("nchar(5)")
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.ScheduleTime).HasColumnType("ntext");

                entity.Property(e => e.StationName).HasColumnType("nchar(30)");

                entity.Property(e => e.TableColumnNamesBackColor).HasColumnType("nchar(30)");

                entity.Property(e => e.TableColumnNamesFontBoldFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.TableColumnNamesFontColor).HasColumnType("nchar(30)");

                entity.Property(e => e.TableHeaderBackColor).HasColumnType("nchar(30)");

                entity.Property(e => e.TableHeaderFontBoldFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.TableHeaderFontColor).HasColumnType("nchar(30)");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

                entity.Property(e => e.TitleContent1).HasColumnType("nchar(150)");

                entity.Property(e => e.TitleContent2).HasColumnType("nchar(150)");

                entity.Property(e => e.TitleContent3).HasColumnType("nchar(150)");

                entity.Property(e => e.TitleFontBoldFlag1).HasColumnType("nchar(5)");

                entity.Property(e => e.TitleFontBoldFlag2).HasColumnType("nchar(5)");

                entity.Property(e => e.TitleFontBoldFlag3).HasColumnType("nchar(5)");

                entity.Property(e => e.TitleFontColor1).HasColumnType("nchar(30)");

                entity.Property(e => e.TitleFontColor2).HasColumnType("nchar(30)");

                entity.Property(e => e.TitleFontColor3).HasColumnType("nchar(30)");
            });

            modelBuilder.Entity<ReportsTemplate>(entity =>
            {
                entity.HasKey(e => e.TemplateId);

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.EnableSchedulerFlag).HasMaxLength(5);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nchar(100)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<ReportsTemplateParameters>(entity =>
            {
                entity.HasKey(e => e.ParameterId);

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.RequiredFlag)
                    .IsRequired()
                    .HasColumnType("nchar(5)")
                    .HasDefaultValueSql("(N'true')");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
            });

            modelBuilder.Entity<ServiceStations>(entity =>
            {
                entity.HasKey(e => e.StationId);

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.BackupFolderId).HasColumnName("BackupFolderID");

                entity.Property(e => e.PdfstationFlag)
                    .HasColumnName("PDFStationFlag")
                    .HasColumnType("nchar(5)");

                entity.Property(e => e.StationName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.TargetFolderId).HasColumnName("TargetFolderID");

                entity.Property(e => e.WatchFolderId).HasColumnName("WatchFolderID");

                entity.Property(e => e.WeekendEndTime).HasMaxLength(22);

                entity.Property(e => e.WeekendFlag).HasColumnType("char(5)");

                entity.Property(e => e.WeekendStartTime).HasMaxLength(22);

                entity.Property(e => e.WorkdayEndTime).HasMaxLength(22);

                entity.Property(e => e.WorkdayFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.WorkdayStartTime).HasMaxLength(22);
            });

            modelBuilder.Entity<Smtp>(entity =>
            {
                entity.ToTable("SMTP");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EnableSslflag)
                    .IsRequired()
                    .HasColumnName("EnableSSLFlag")
                    .HasColumnType("nchar(5)")
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.HostName)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.Password).HasColumnType("nchar(30)");

                entity.Property(e => e.SenderEmailAddress)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.SenderName).HasColumnType("nchar(50)");

                entity.Property(e => e.UserName).HasColumnType("nchar(30)");
            });

            modelBuilder.Entity<Sssfunctionality>(entity =>
            {
                entity.HasKey(e => e.FunctionalityId);

                entity.ToTable("SSSFunctionality");

                entity.Property(e => e.FunctionalityId).HasColumnName("FunctionalityID");

                entity.Property(e => e.Functionality)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ActiveFlag).HasColumnType("nchar(5)");

                entity.Property(e => e.ApproveBatches).HasColumnType("char(10)");

                entity.Property(e => e.CreateBatches).HasColumnType("char(10)");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasColumnType("nchar(30)");

                entity.Property(e => e.ExportBatches).HasColumnType("char(10)");

                entity.Property(e => e.FirstName).HasColumnType("nchar(20)");

                entity.Property(e => e.GenerateBatchReport).HasColumnType("char(10)");

                entity.Property(e => e.GenerateOperationReports).HasColumnType("char(10)");

                entity.Property(e => e.LastName).HasColumnType("nchar(20)");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QualityControl)
                    .HasColumnType("nchar(10)")
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.RejectBatches).HasColumnType("char(10)");

                entity.Property(e => e.ReturnBatches)
                    .HasColumnType("nchar(10)")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.ScanBatches).HasColumnType("char(10)");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UploadBatches).HasColumnType("char(10)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ViewBatchInformation).HasColumnType("char(10)");

                entity.Property(e => e.ViewDocumentInformation).HasColumnType("char(10)");
            });

            modelBuilder.Entity<UserUifunctionality>(entity =>
            {
                entity.HasKey(e => new { e.FunctionalityId, e.UserId });

                entity.ToTable("UserUIFunctionality");

                entity.Property(e => e.FunctionalityId).HasColumnName("FunctionalityID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Vfr>(entity =>
            {
                entity.HasKey(e => e.SettingId);

                entity.ToTable("VFR");

                entity.Property(e => e.SettingId).HasColumnName("SettingID");

                entity.Property(e => e.Cadiurl)
                    .IsRequired()
                    .HasColumnName("CADIUrl")
                    .IsUnicode(false);

                entity.Property(e => e.CaptureTemplate)
                    .IsRequired()
                    .HasColumnType("nchar(100)");

                entity.Property(e => e.InstanceName)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("nchar(30)");

                entity.Property(e => e.QueryField)
                    .IsRequired()
                    .HasColumnType("nchar(30)");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("nchar(30)");
            });

            modelBuilder.Entity<Vfrdocs>(entity =>
            {
                entity.HasKey(e => new { e.BatchNumber, e.DocumentId });

                entity.ToTable("VFRDocs");

                entity.Property(e => e.BatchNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentId)
                    .HasColumnName("DocumentID")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.JobId)
                    .IsRequired()
                    .HasColumnName("JobID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusFlag)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WorkingFolders>(entity =>
            {
                entity.HasKey(e => e.FolderId);

                entity.Property(e => e.Path).HasColumnType("ntext");
            });
        }
    }
}
