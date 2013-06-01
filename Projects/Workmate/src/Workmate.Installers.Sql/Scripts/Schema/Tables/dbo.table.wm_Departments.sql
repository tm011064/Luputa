IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Departments](
	[ApplicationId] [int] NOT NULL,
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[ParentDepartmentId] [int] NULL,
	[Name] [nvarchar](256) NOT NULL,
	[OfficeId] [int] NULL,
 CONSTRAINT [PK_wm_Departments] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO