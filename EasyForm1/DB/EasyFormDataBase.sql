CREATE TABLE [dbo].[Forms] (
    [FormID]        INT           IDENTITY (1, 1) NOT NULL,
    [FormName]      VARCHAR (100) NOT NULL,
    [LastUsing]     DATE          DEFAULT (getdate()) NOT NULL,
    [AmountOfUsing] INT           DEFAULT ((0)) NOT NULL,
    [UserID]        INT           NOT NULL,
    [Sharing]       BIT           NOT NULL,
	[ImagePath] varchar(200) not null,
    PRIMARY KEY CLUSTERED ([FormID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

