create procedure testInsert
@tablename nvarchar(20),@value int
as
begin
declare @sql nvarchar(800);
set @sql='insert into '+@tablename+' values ('+convert(nvarchar(20), @value)+')';
exec(@sql);
end
