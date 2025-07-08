
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NotesConfiguration](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[FacilityId] [uniqueidentifier] NOT NULL,
	[SpecificationId] [uniqueidentifier] NOT NULL,
	[FileName] [varchar](255) NOT NULL,
	[UploadedBy] [nvarchar](100) NOT NULL,
	[UploadedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](100) NULL,
	[ModifiedOn] [datetime] NULL,
	[FileData] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_NotesConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[NotesConfiguration] ADD  CONSTRAINT [DF_NotesConfiguration_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[NotesConfiguration] ADD  CONSTRAINT [DF_NotesConfiguration_UploadedOn]  DEFAULT (getdate()) FOR [UploadedOn]
GO

ALTER TABLE [dbo].[NotesConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_NotesConfig_Facility] FOREIGN KEY([FacilityId])
REFERENCES [dbo].[Facility] ([Id])
GO

ALTER TABLE [dbo].[NotesConfiguration] CHECK CONSTRAINT [FK_NotesConfig_Facility]
GO

ALTER TABLE [dbo].[NotesConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_NotesConfig_Specification] FOREIGN KEY([SpecificationId])
REFERENCES [dbo].[Specification] ([Id])
GO

ALTER TABLE [dbo].[NotesConfiguration] CHECK CONSTRAINT [FK_NotesConfig_Specification]
GO


