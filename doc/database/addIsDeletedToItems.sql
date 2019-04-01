ALTER TABLE items add isDeleted bit not null default 0
update items set isDeleted=0