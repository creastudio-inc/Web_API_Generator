using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_API_Generator.Models
{
    public enum dataFieldType
    {
        Int,
        String,
        Datetime,
        Double,
        Bool,
        Object
    }
    public enum DataAnnotation
    {
        Required,
        DataType,
        Range,
        StringLength,
        MaxLength,
        RegularExpression
    }

    public class ProjectViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public virtual ICollection<TableViewModels> TableViewModels { get; set; }

    }
    public class TableViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public String DataFieldType { get; set; } // mobiles / Models 

        [ForeignKey("Project")]
        public int ProjectID { get; set; }
        public virtual ProjectViewModels Project { get; set; }

        public virtual ICollection<DataFieldForeignKeyViewModels> dataFieldForeignKeyViewModels { get; set; }

        public virtual ICollection<DataFieldViewModels> DataFieldViewModels { get; set; }
    }

    public class DataFieldViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Name { get; set; }
        public Web_API_Generator.Models.dataFieldType Type { get; set; }
        public String Lengh { get; set; }
        public bool Nullable { get; set; }
       // public bool IsEnum { get; set; }

        [ForeignKey("Table")]
        public int TableId { get; set; }
        public virtual TableViewModels Table { get; set; }

        public virtual ICollection<DataAnnotationViewModels> DataAnnotationViewModels { get; set; }
    }

    public class DataFieldForeignKeyViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Name { get; set; }
        public bool Required { get; set; }

        [ForeignKey("DataFieldView")]
        public int? DataFieldViewID { get; set; }
        public virtual TableViewModels DataFieldView { get; set; }

        [ForeignKey("TableView")]
        public int? TableViewID { get; set; }
        public virtual TableViewModels TableView { get; set; }
    }

    public class DataAnnotationViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DataAnnotation Name { get; set; }
        public String Param1 { get; set; }
        public String Param2 { get; set; }
        public String ErrorMessage { get; set; }

        [ForeignKey("DataField")]
        public int DataFieldId { get; set; }
        public virtual DataFieldViewModels DataField { get; set; }

    }

    public class EnumViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Name { get; set; }
        public String Descriptions { get; set; }

        public virtual ICollection<EnumDetailsViewModels> EnumDetailsViewModels { get; set; }
    }

    public class EnumDetailsViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Name { get; set; }
    }
}