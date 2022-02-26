namespace GestionFicha.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracionnum124022022 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ADMINISTRADORES",
                c => new
                    {
                        ID_PERSONA = c.Decimal(nullable: false, precision: 5, scale: 0, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.ID_PERSONA)
                .ForeignKey("dbo.PERSONAL", t => t.ID_PERSONA)
                .Index(t => t.ID_PERSONA);
            
            CreateTable(
                "dbo.PERSONAL",
                c => new
                    {
                        N_INTERNO = c.Decimal(nullable: false, precision: 5, scale: 0, storeType: "numeric"),
                        NOMBRE = c.String(maxLength: 60, unicode: false),
                        APELLIDOS = c.String(maxLength: 60, unicode: false),
                        USUARIO_RED = c.String(maxLength: 15, unicode: false),
                        N_INTERNO_RESP = c.Decimal(precision: 5, scale: 0, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.N_INTERNO)
                .ForeignKey("dbo.PERSONAL", t => t.N_INTERNO_RESP)
                .Index(t => t.N_INTERNO_RESP);
            
            CreateTable(
                "dbo.GESTORES",
                c => new
                    {
                        ID_PERSONA = c.Decimal(nullable: false, precision: 5, scale: 0, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.ID_PERSONA)
                .ForeignKey("dbo.PERSONAL", t => t.ID_PERSONA)
                .Index(t => t.ID_PERSONA);
            
            CreateTable(
                "dbo.ORDEN",
                c => new
                    {
                        ID_ORDEN = c.Int(nullable: false, identity: true),
                        FECHA = c.DateTime(nullable: false),
                        DIRECCION = c.String(nullable: false),
                        CANTIDAD = c.Int(nullable: false),
                        ES_GESTOR = c.Boolean(nullable: false),
                        ID_PRODUCTO = c.Int(nullable: false),
                        N_INTERNO = c.Decimal(nullable: false, precision: 5, scale: 0, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.ID_ORDEN)
                .ForeignKey("dbo.PERSONAL", t => t.N_INTERNO, cascadeDelete: true)
                .ForeignKey("dbo.PRODUCTO", t => t.ID_PRODUCTO, cascadeDelete: true)
                .Index(t => t.ID_PRODUCTO)
                .Index(t => t.N_INTERNO);
            
            CreateTable(
                "dbo.PRODUCTO",
                c => new
                    {
                        ID_PRODUCTO = c.Int(nullable: false, identity: true),
                        NOMBRE = c.String(nullable: false, maxLength: 150),
                        DESCRIPCION = c.String(maxLength: 60),
                        PRECIO = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.ID_PRODUCTO);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PERSONAL", "N_INTERNO_RESP", "dbo.PERSONAL");
            DropForeignKey("dbo.ORDEN", "ID_PRODUCTO", "dbo.PRODUCTO");
            DropForeignKey("dbo.ORDEN", "N_INTERNO", "dbo.PERSONAL");
            DropForeignKey("dbo.GESTORES", "ID_PERSONA", "dbo.PERSONAL");
            DropForeignKey("dbo.ADMINISTRADORES", "ID_PERSONA", "dbo.PERSONAL");
            DropIndex("dbo.ORDEN", new[] { "N_INTERNO" });
            DropIndex("dbo.ORDEN", new[] { "ID_PRODUCTO" });
            DropIndex("dbo.GESTORES", new[] { "ID_PERSONA" });
            DropIndex("dbo.PERSONAL", new[] { "N_INTERNO_RESP" });
            DropIndex("dbo.ADMINISTRADORES", new[] { "ID_PERSONA" });
            DropTable("dbo.PRODUCTO");
            DropTable("dbo.ORDEN");
            DropTable("dbo.GESTORES");
            DropTable("dbo.PERSONAL");
            DropTable("dbo.ADMINISTRADORES");
        }
    }
}
