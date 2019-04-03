use Coliks
GO

Create Table Brands (
	id int identity primary key,
	brandname varchar(50) unique
)
GO

Insert into Brands select distinct brand from items

alter table items add brand_id int
GO
update items set brand_id = (select id from Brands where brandname = brand)
GO
ALTER TABLE items DROP CONSTRAINT DF__items__brand__52593CB8
alter table items drop column brand

alter table items add constraint fk_brand foreign key (brand_id) references brands(id)
