
/****** Object:  Table [dbo].[WelcomeMessage]    Script Date: 2025-03-20 10:41:55 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WelcomeMessage](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[Message2] [nvarchar](max) NULL,
	[Message3] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_WelcomeMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[WelcomeMessage] ADD  CONSTRAINT [DF_WelcomeMessage_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[WelcomeMessage] ADD  CONSTRAINT [DF_WelcomeMessage_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[WelcomeMessage] ADD  CONSTRAINT [DF_WelcomeMessage_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO


INSERT INTO [dbo].[WelcomeMessage] (
    [Description],
    [Notes],
    [Message2],
    [Message3],
    [CreatedBy],
    [ModifiedBy]
)
VALUES (
    N'Line List Application is only for Christina Lake, Foster Creek, Bruderheim, and ZBD well pads process piping information and Line Designation Tables (LDTs). All line record and LDT modifications MUST be issued from this application. Please refer to the User Manual as well as CVE-10-PRC-00-0046-001 Issuing Line Designation Tables.',    
    N'EDIT LINES IN GRID function has been replaced with GRID EXPORT / GRID IMPORT functionality.Refer to updated User Guide section 6.12.12.',    
    N'PRINT AND ISSUE functions have changed. Please refer to User Guide Sections 6.07 – 6.09.',   
    N'If you are having difficulty locating the stamped PDFs of the LDTs for these process piping records in McLaren, contact linelist.linelist@cenovus.com. For access requests and issues, please contact Service Desk.',    
    N'System',                   
    N'System'                    
);
