﻿create table ItemsTb1(ItCode int primary key identity,ItName varchar(80),ItCategory int foreign key references CategoryTb1(CatCode),Price varchar(15),Stock varchar(15));