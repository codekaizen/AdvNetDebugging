
create table [dbo].[Metrics](
	[FoodName] [nvarchar](1000) not null,
	[DocumentUri] [nvarchar](1000) not null,
	[ComparisonFoodName] [nvarchar](1000) not null,
	[Norm] [float] not null
);