using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public partial class SystemdatabaseContext : DbContext
{
    public SystemdatabaseContext()
    {
    }

    public SystemdatabaseContext(DbContextOptions<SystemdatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnalysisDepartment> AnalysisDepartments { get; set; }

    public virtual DbSet<AnalysisWork> AnalysisWorks { get; set; }

    public virtual DbSet<Analysise> Analysises { get; set; }

    public virtual DbSet<AnalysisesTemplate> AnalysisesTemplates { get; set; }

    public virtual DbSet<BarcodeAnalysise> BarcodeAnalysises { get; set; }

    public virtual DbSet<BarcodeMaterial> BarcodeMaterials { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractAnalysise> ContractAnalysises { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Lpu> Lpus { get; set; }

    public virtual DbSet<LpuContract> LpuContracts { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Measurement> Measurements { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderChange> OrderChanges { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientChange> PatientChanges { get; set; }

    public virtual DbSet<QualitativeStandart> QualitativeStandarts { get; set; }

    public virtual DbSet<QuantitativeStandart> QuantitativeStandarts { get; set; }

    public virtual DbSet<ReferentialGroup> ReferentialGroups { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tripod> Tripods { get; set; }

    public virtual DbSet<TripodBarcodeMaterial> TripodBarcodeMaterials { get; set; }

    public virtual DbSet<TypeChange> TypeChanges { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnalysisDepartment>(entity =>
        {
            entity.HasKey(e => e.AnalysisDepId).HasName("departments_pkey");

            entity.ToTable("analysis_departments", "Lab");

            entity.Property(e => e.AnalysisDepId)
                .ValueGeneratedNever()
                .HasColumnName("analysis_dep_id");
            entity.Property(e => e.AnalysisDepName).HasColumnName("analysis_dep_name");
        });

        modelBuilder.Entity<AnalysisWork>(entity =>
        {
            entity.HasKey(e => e.AnalysisWorkId).HasName("analysis_works_pkey");

            entity.ToTable("analysis_works", "Lab");

            entity.Property(e => e.AnalysisWorkId)
                .HasDefaultValueSql("nextval('\"Lab\".analysis_works_analysis_work_id_seq1'::regclass)")
                .HasColumnName("analysis_work_id");
            entity.Property(e => e.AnalysisId).HasColumnName("analysis_id");
            entity.Property(e => e.AnalysisWorkName).HasColumnName("analysis_work_name");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");

            entity.HasOne(d => d.Analysis).WithMany(p => p.AnalysisWorks)
                .HasForeignKey(d => d.AnalysisId)
                .HasConstraintName("analysis_works_analysis_id_fkey");

            entity.HasOne(d => d.Material).WithMany(p => p.AnalysisWorks)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("analysis_works_material_id_fkey");
        });

        modelBuilder.Entity<Analysise>(entity =>
        {
            entity.HasKey(e => e.AnalysisId).HasName("analysises_pkey");

            entity.ToTable("analysises", "Lab");

            entity.Property(e => e.AnalysisId)
                .HasDefaultValueSql("nextval('\"Lab\".analysises_analysis_id_seq1'::regclass)")
                .HasColumnName("analysis_id");
            entity.Property(e => e.AnalysisCodeName).HasColumnName("analysis_code_name");
            entity.Property(e => e.AnalysisDepId).HasColumnName("analysis_dep_id");
            entity.Property(e => e.AnalysisName).HasColumnName("analysis_name");

            entity.HasOne(d => d.AnalysisDep).WithMany(p => p.Analysises)
                .HasForeignKey(d => d.AnalysisDepId)
                .HasConstraintName("analysises_dep_id_fkey");
        });

        modelBuilder.Entity<AnalysisesTemplate>(entity =>
        {
            entity.HasKey(e => new { e.AnalysisTempId, e.AnalysisId }).HasName("analysises_temp_id_pkey");

            entity.ToTable("analysises_templates", "Lab");

            entity.Property(e => e.AnalysisTempId).HasColumnName("analysis_temp_id");
            entity.Property(e => e.AnalysisId).HasColumnName("analysis_id");
            entity.Property(e => e.AnalysisTempName).HasColumnName("analysis_temp_name");

            entity.HasOne(d => d.Analysis).WithMany(p => p.AnalysisesTemplates)
                .HasForeignKey(d => d.AnalysisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("analysises_templates_analysis_id_fkey");
        });

        modelBuilder.Entity<BarcodeAnalysise>(entity =>
        {
            entity.HasKey(e => new { e.BarcodeId, e.AnalysisId }).HasName("barcode_analysis_pk");

            entity.ToTable("barcode_analysises", "Lab");

            entity.Property(e => e.BarcodeId)
                .HasPrecision(11)
                .HasColumnName("barcode_id");
            entity.Property(e => e.AnalysisId).HasColumnName("analysis_id");
            entity.Property(e => e.AnalysisDepId).HasColumnName("analysis_dep_id");
            entity.Property(e => e.Result)
                .HasColumnType("json")
                .HasColumnName("result");

            entity.HasOne(d => d.Analysis).WithMany(p => p.BarcodeAnalysises)
                .HasForeignKey(d => d.AnalysisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("barcode_analysises_analysis_id_fkey");

            entity.HasOne(d => d.BarcodeMaterial).WithMany(p => p.BarcodeAnalysises)
                .HasForeignKey(d => new { d.BarcodeId, d.AnalysisDepId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_barcode_analysises_barcode_id");
        });

        modelBuilder.Entity<BarcodeMaterial>(entity =>
        {
            entity.HasKey(e => new { e.BarcodeMatId, e.AnalysisDepId }).HasName("barcode_mat_id_analysis_dep_pkey");

            entity.ToTable("barcode_materials", "Lab");

            entity.Property(e => e.BarcodeMatId)
                .HasPrecision(11)
                .HasColumnName("barcode_mat_id");
            entity.Property(e => e.AnalysisDepId).HasColumnName("analysis_dep_id");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");

            entity.HasOne(d => d.AnalysisDep).WithMany(p => p.BarcodeMaterials)
                .HasForeignKey(d => d.AnalysisDepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("barcode_materials_analysis_dep_id_fkey");

            entity.HasOne(d => d.Material).WithMany(p => p.BarcodeMaterials)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("barcode_materials_material_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.BarcodeMaterials)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("barcode_materials_order_id_fkey");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("contracts_pkey");

            entity.ToTable("contracts", "Lab");

            entity.Property(e => e.ContractId)
                .HasDefaultValueSql("nextval('\"Lab\".contracts_contract_id_seq1'::regclass)")
                .HasColumnName("contract_id");
            entity.Property(e => e.ContractMoney).HasColumnName("contract_money");
            entity.Property(e => e.ContractName).HasColumnName("contract_name");
            entity.Property(e => e.ContractRemainsMoney).HasColumnName("contract_remains_money");
        });

        modelBuilder.Entity<ContractAnalysise>(entity =>
        {
            entity.HasKey(e => new { e.ContractId, e.AnalysisId }).HasName("contract_analysis_pk");

            entity.ToTable("contract_analysises", "Lab");

            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.AnalysisId).HasColumnName("analysis_id");
            entity.Property(e => e.ContrAnalysisCost).HasColumnName("contr_analysis_cost");

            entity.HasOne(d => d.Analysis).WithMany(p => p.ContractAnalysises)
                .HasForeignKey(d => d.AnalysisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_analysises_analysis_id_fkey");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractAnalysises)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_analysises_contract_id_fkey");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("doctors_pkey");

            entity.ToTable("doctors", "Lab");

            entity.Property(e => e.DocId)
                .HasDefaultValueSql("nextval('\"Lab\".doctors_doc_id_seq1'::regclass)")
                .HasColumnName("doc_id");
            entity.Property(e => e.DocFio).HasColumnName("doc_fio");
            entity.Property(e => e.LpuId).HasColumnName("lpu_id");

            entity.HasOne(d => d.Lpu).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.LpuId)
                .HasConstraintName("doctors_lpu_id_fkey");
        });

        modelBuilder.Entity<Lpu>(entity =>
        {
            entity.HasKey(e => e.LpuId).HasName("lpus_pkey");

            entity.ToTable("lpus", "Lab");

            entity.Property(e => e.LpuId)
                .HasDefaultValueSql("nextval('\"Lab\".lpus_lpu_id_seq1'::regclass)")
                .HasColumnName("lpu_id");
            entity.Property(e => e.LpuEmail).HasColumnName("lpu_email");
            entity.Property(e => e.LpuName).HasColumnName("lpu_name");
        });

        modelBuilder.Entity<LpuContract>(entity =>
        {
            entity.HasKey(e => e.ConLpuId).HasName("lpu_contracts_pkey");

            entity.ToTable("lpu_contracts", "Lab");

            entity.Property(e => e.ConLpuId)
                .HasDefaultValueSql("nextval('\"Lab\".lpu_contracts_con_lpu_id_seq1'::regclass)")
                .HasColumnName("con_lpu_id");
            entity.Property(e => e.ConLpuIsActive).HasColumnName("con_lpu_is_active");
            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.LpuId).HasColumnName("lpu_id");

            entity.HasOne(d => d.Contract).WithMany(p => p.LpuContracts)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lpu_contracts_contract_id_fkey");

            entity.HasOne(d => d.Lpu).WithMany(p => p.LpuContracts)
                .HasForeignKey(d => d.LpuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lpu_contracts_lpu_id_fkey");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("materials_pkey");

            entity.ToTable("materials", "Lab");

            entity.Property(e => e.MaterialId)
                .HasDefaultValueSql("nextval('\"Lab\".materials_material_id_seq1'::regclass)")
                .HasColumnName("material_id");
            entity.Property(e => e.MaterialName).HasColumnName("material_name");
        });

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.HasKey(e => e.MeasurementId).HasName("measurements_pkey");

            entity.ToTable("measurements", "Lab");

            entity.Property(e => e.MeasurementId)
                .HasDefaultValueSql("nextval('\"Lab\".measurements_measurement_id_seq1'::regclass)")
                .HasColumnName("measurement_id");
            entity.Property(e => e.MeasurementName).HasColumnName("measurement_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders", "Lab");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("nextval('\"Lab\".order_order_id_seq'::regclass)")
                .HasColumnName("order_id");
            entity.Property(e => e.DocId).HasColumnName("doc_id");
            entity.Property(e => e.LpuId).HasColumnName("lpu_id");
            entity.Property(e => e.OrderIsCountingInContract).HasColumnName("order_is_counting_in_contract");
            entity.Property(e => e.OrderLpuDepartment).HasColumnName("order_lpu_department");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");

            entity.HasOne(d => d.Doc).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DocId)
                .HasConstraintName("orders_doc_id_fkey");

            entity.HasOne(d => d.Lpu).WithMany(p => p.Orders)
                .HasForeignKey(d => d.LpuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_lpu_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_patient_id_fkey");

            entity.HasMany(d => d.ConLpus).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderContract",
                    r => r.HasOne<LpuContract>().WithMany()
                        .HasForeignKey("ConLpuId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("order_contracts_con_lpu_id_fkey"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("order_contracts_order_id_fkey"),
                    j =>
                    {
                        j.HasKey("OrderId", "ConLpuId").HasName("order_contracts_pk");
                        j.ToTable("order_contracts", "Lab");
                        j.IndexerProperty<long>("OrderId").HasColumnName("order_id");
                        j.IndexerProperty<long>("ConLpuId").HasColumnName("con_lpu_id");
                    });
        });

        modelBuilder.Entity<OrderChange>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.WorkerId, e.OrderChangeTime }).HasName("order_changes_pkey");

            entity.ToTable("order_changes", "Lab");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");
            entity.Property(e => e.OrderChangeTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_change_time");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderChanges)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_changes_order_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.OrderChanges)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_changes_type_id_fkey");

            entity.HasOne(d => d.Worker).WithMany(p => p.OrderChanges)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_changes_worker_id_fkey");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("patients_pkey");

            entity.ToTable("patients", "Lab");

            entity.Property(e => e.PatientId)
                .HasDefaultValueSql("nextval('\"Lab\".patients_patient_id_seq1'::regclass)")
                .HasColumnName("patient_id");
            entity.Property(e => e.PatientBirthday).HasColumnName("patient_birthday");
            entity.Property(e => e.PatientEmail).HasColumnName("patient_email");
            entity.Property(e => e.PatientFirstName).HasColumnName("patient_first_name");
            entity.Property(e => e.PatientGender).HasColumnName("patient_gender");
            entity.Property(e => e.PatientLastName).HasColumnName("patient_last_name");
            entity.Property(e => e.PatientSecondName).HasColumnName("patient_second_name");
        });

        modelBuilder.Entity<PatientChange>(entity =>
        {
            entity.HasKey(e => new { e.PatientId, e.WorkerId, e.PatientChangeTime }).HasName("patient_changes_pkey");

            entity.ToTable("patient_changes", "Lab");

            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");
            entity.Property(e => e.PatientChangeTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("patient_change_time");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Patient).WithMany(p => p.PatientChanges)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("patient_changes_patient_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.PatientChanges)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("patient_changes_type_id_fkey");

            entity.HasOne(d => d.Worker).WithMany(p => p.PatientChanges)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("patient_changes_worker_id_fkey");
        });

        modelBuilder.Entity<QualitativeStandart>(entity =>
        {
            entity.HasKey(e => e.QualtityStandartId).HasName("qualitative_standarts_pkey");

            entity.ToTable("qualitative_standarts", "Lab");

            entity.Property(e => e.QualtityStandartId)
                .HasDefaultValueSql("nextval('\"Lab\".qualitative_standarts_qualtity_standart_id_seq1'::regclass)")
                .HasColumnName("qualtity_standart_id");
            entity.Property(e => e.AnalysisWorkId).HasColumnName("analysis_work_id");
            entity.Property(e => e.QualityStandartCondition).HasColumnName("quality_standart_condition");
            entity.Property(e => e.QualityStandartDescription).HasColumnName("quality_standart_description");
            entity.Property(e => e.QualityStandartTypeCodition).HasColumnName("quality_standart_type_codition");
            entity.Property(e => e.RefGroupId).HasColumnName("ref_group_id");

            entity.HasOne(d => d.AnalysisWork).WithMany(p => p.QualitativeStandarts)
                .HasForeignKey(d => d.AnalysisWorkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qualitative_standarts_analysis_work_id_fkey");

            entity.HasOne(d => d.RefGroup).WithMany(p => p.QualitativeStandarts)
                .HasForeignKey(d => d.RefGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qualitative_standarts_ref_group_id_fkey");
        });

        modelBuilder.Entity<QuantitativeStandart>(entity =>
        {
            entity.HasKey(e => e.QuantStandartId).HasName("quantitative_standarts_pkey");

            entity.ToTable("quantitative_standarts", "Lab");

            entity.Property(e => e.QuantStandartId)
                .HasDefaultValueSql("nextval('\"Lab\".quantitative_standarts_quant_standart_id_seq1'::regclass)")
                .HasColumnName("quant_standart_id");
            entity.Property(e => e.AnalysisWorkId).HasColumnName("analysis_work_id");
            entity.Property(e => e.MeasurementsId).HasColumnName("measurements_id");
            entity.Property(e => e.QuantStandartDescription).HasColumnName("quant_standart_description");
            entity.Property(e => e.QuantStandartHighCritical).HasColumnName("quant_standart_high_critical");
            entity.Property(e => e.QuantStandartHighNorm).HasColumnName("quant_standart_high_norm");
            entity.Property(e => e.QuantStandartHighPathology).HasColumnName("quant_standart_high_pathology");
            entity.Property(e => e.QuantStandartLowCritical).HasColumnName("quant_standart_low_critical");
            entity.Property(e => e.QuantStandartLowNorm).HasColumnName("quant_standart_low_norm");
            entity.Property(e => e.QuantStandartLowPathology).HasColumnName("quant_standart_low_pathology");
            entity.Property(e => e.RefGroupId).HasColumnName("ref_group_id");

            entity.HasOne(d => d.AnalysisWork).WithMany(p => p.QuantitativeStandarts)
                .HasForeignKey(d => d.AnalysisWorkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quantitative_standarts_analysis_work_id_fkey");

            entity.HasOne(d => d.Measurements).WithMany(p => p.QuantitativeStandarts)
                .HasForeignKey(d => d.MeasurementsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quantitative_standarts_measurements_id_fkey");

            entity.HasOne(d => d.RefGroup).WithMany(p => p.QuantitativeStandarts)
                .HasForeignKey(d => d.RefGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quantitative_standarts_ref_group_id_fkey");
        });

        modelBuilder.Entity<ReferentialGroup>(entity =>
        {
            entity.HasKey(e => e.RefGroupId).HasName("referential_groups_pkey");

            entity.ToTable("referential_groups", "Lab");

            entity.Property(e => e.RefGroupId)
                .HasDefaultValueSql("nextval('\"Lab\".referential_groups_ref_group_id_seq1'::regclass)")
                .HasColumnName("ref_group_id");
            entity.Property(e => e.RefGroupCondition).HasColumnName("ref_group_condition");
            entity.Property(e => e.RefGroupGender).HasColumnName("ref_group_gender");
            entity.Property(e => e.RefGroupHighAge).HasColumnName("ref_group_high_age");
            entity.Property(e => e.RefGroupHighIf).HasColumnName("ref_group_high_if");
            entity.Property(e => e.RefGroupLowAge).HasColumnName("ref_group_low_age");
            entity.Property(e => e.RefGroupLowIf).HasColumnName("ref_group_low_if");
            entity.Property(e => e.RefGroupName).HasColumnName("ref_group_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles", "Lab");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<Tripod>(entity =>
        {
            entity.HasKey(e => e.TripodId).HasName("tripods_pkey");

            entity.ToTable("tripods", "Lab");

            entity.Property(e => e.TripodId)
                .HasDefaultValueSql("nextval('\"Lab\".tripods_tripod_id_seq1'::regclass)")
                .HasColumnName("tripod_id");
            entity.Property(e => e.TripodCreateDate).HasColumnName("tripod_create_date");
            entity.Property(e => e.TripodMaxCell).HasColumnName("tripod_max_cell");
            entity.Property(e => e.TripodName).HasColumnName("tripod_name");
        });

        modelBuilder.Entity<TripodBarcodeMaterial>(entity =>
        {
            entity.HasKey(e => new { e.TripodId, e.BarcodeMatId }).HasName("tripod_barcode_material_pkey");

            entity.ToTable("tripod_barcode_materials", "Lab");

            entity.Property(e => e.TripodId).HasColumnName("tripod_id");
            entity.Property(e => e.BarcodeMatId)
                .HasPrecision(11)
                .HasColumnName("barcode_mat_id");
            entity.Property(e => e.AnalysisDepId).HasColumnName("analysis_dep_id");

            entity.HasOne(d => d.Tripod).WithMany(p => p.TripodBarcodeMaterials)
                .HasForeignKey(d => d.TripodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tripod_barcode_materials_tripod_id_fkey");

            entity.HasOne(d => d.BarcodeMaterial).WithMany(p => p.TripodBarcodeMaterials)
                .HasForeignKey(d => new { d.BarcodeMatId, d.AnalysisDepId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tripod_barcode_materials_barcode_mat_id");
        });

        modelBuilder.Entity<TypeChange>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("type_change_pkey");

            entity.ToTable("type_change", "Lab");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("type_id");
            entity.Property(e => e.TypeName).HasColumnName("type_name");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("workers_pkey");

            entity.ToTable("workers", "Lab");

            entity.Property(e => e.WorkerId)
                .HasDefaultValueSql("nextval('\"Lab\".workers_worker_id_seq1'::regclass)")
                .HasColumnName("worker_id");
            entity.Property(e => e.WorkerFio).HasColumnName("worker_fio");
            entity.Property(e => e.WorkerLogin).HasColumnName("worker_login");
            entity.Property(e => e.WorkerPassword).HasColumnName("worker_password");

            entity.HasMany(d => d.Roles).WithMany(p => p.Workers)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkerRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("worker_role_role_id_fkey"),
                    l => l.HasOne<Worker>().WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("worker_role_worker_id_fkey"),
                    j =>
                    {
                        j.HasKey("WorkerId", "RoleId").HasName("worker_role_pk_key");
                        j.ToTable("worker_role", "Lab");
                        j.IndexerProperty<int>("WorkerId").HasColumnName("worker_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    });
        });
        modelBuilder.HasSequence("analysis_works_analysis_work_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("analysises_analysis_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("contracts_contract_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("doctors_doc_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("lpu_contracts_con_lpu_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("lpus_lpu_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("materials_material_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("measurements_measurement_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("order_order_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("patients_patient_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("patterns_pattern_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("qualitative_standarts_qualtity_standart_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("quantitative_standarts_quant_standart_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("referential_groups_ref_group_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("tripods_tripod_id_seq", "Lab").HasMax(2147483647L);
        modelBuilder.HasSequence("workers_worker_id_seq", "Lab").HasMax(2147483647L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
