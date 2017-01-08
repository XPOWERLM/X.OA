
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/25/2016 21:50:49
-- Generated from EDMX file: C:\Users\x9921\OneDrive\code\C#\X.OA\X.OA.Model\DFSModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [OA.DFS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ServerInfoImageInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImageInfoes] DROP CONSTRAINT [FK_ServerInfoImageInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ServerInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServerInfoes];
GO
IF OBJECT_ID(N'[dbo].[ImageInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ImageInfoes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ServerInfoes'
CREATE TABLE [dbo].[ServerInfoes] (
    [ServerId] int IDENTITY(1,1) NOT NULL,
    [ServerName] nvarchar(max)  NOT NULL,
    [ServerUrl] nvarchar(max)  NOT NULL,
    [PicPath] nvarchar(max)  NOT NULL,
    [MaxAmount] int  NOT NULL,
    [CurrentAmount] int  NOT NULL,
    [IsAlive] bit  NOT NULL
);
GO

-- Creating table 'ImageInfoes'
CREATE TABLE [dbo].[ImageInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ImageName] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL,
    [ServerInfoServerId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ServerId] in table 'ServerInfoes'
ALTER TABLE [dbo].[ServerInfoes]
ADD CONSTRAINT [PK_ServerInfoes]
    PRIMARY KEY CLUSTERED ([ServerId] ASC);
GO

-- Creating primary key on [Id] in table 'ImageInfoes'
ALTER TABLE [dbo].[ImageInfoes]
ADD CONSTRAINT [PK_ImageInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ServerInfoServerId] in table 'ImageInfoes'
ALTER TABLE [dbo].[ImageInfoes]
ADD CONSTRAINT [FK_ServerInfoImageInfo]
    FOREIGN KEY ([ServerInfoServerId])
    REFERENCES [dbo].[ServerInfoes]
        ([ServerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServerInfoImageInfo'
CREATE INDEX [IX_FK_ServerInfoImageInfo]
ON [dbo].[ImageInfoes]
    ([ServerInfoServerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------