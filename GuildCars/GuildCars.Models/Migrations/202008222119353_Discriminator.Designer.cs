﻿// <auto-generated />
namespace GuildCars.Models.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class Discriminator : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(Discriminator));
        
        string IMigrationMetadata.Id
        {
            get { return "202008222119353_Discriminator"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}