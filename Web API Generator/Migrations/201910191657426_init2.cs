namespace Web_API_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DataFieldForeignKeyViewModels", name: "TableId", newName: "TableViewID");
            RenameIndex(table: "dbo.DataFieldForeignKeyViewModels", name: "IX_TableId", newName: "IX_TableViewID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DataFieldForeignKeyViewModels", name: "IX_TableViewID", newName: "IX_TableId");
            RenameColumn(table: "dbo.DataFieldForeignKeyViewModels", name: "TableViewID", newName: "TableId");
        }
    }
}
