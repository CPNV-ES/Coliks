Use Coliks
GO

Create Table customer_categories (
	id int identity primary key,
	categoryname varchar(20)
)
GO

alter table customers add category_id int
GO
alter table customers add constraint fk_cat foreign key (category_id) references customer_categories(id)

