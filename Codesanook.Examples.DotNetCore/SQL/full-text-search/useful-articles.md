https://www.sqlskills.com/blogs/joe/combining-multiple-contains-predicates-sql-server-2012/


-- http://hodentekhelp.blogspot.com/2017/05/how-do-you-create-full-text-catalog-to.html
-- https://www.sqlskills.com/blogs/joe/combining-multiple-contains-predicates-sql-server-2012/


-- install full text search
-- https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup-full-text-search?view=sql-server-ver15#install-on-ubuntu
-- apt-get install -y mssql-server-fts
/*
apt-get update
apt-get install -y curl

curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
curl https://packages.microsoft.com/config/ubuntu/16.04/mssql-server-2017.list | tee /etc/apt/sources.list.d/mssql-server.list

apt-get update
apt-get install -y mssql-server-fts
*/


/*
install full text search
https://schwabencode.com/blog/2019/10/27/MSSQL-Server-2017-Docker-Full-Text-Search
https://gist.github.com/avernet/a8a79ba9835056d9456c55441a602184
*/


CREATE FULLTEXT CATALOG AW2008FullTextCatalog
DROP FULLTEXT CATALOG  AW2008FullTextCatalog  

CREATE FULLTEXT INDEX ON [Production].[ProductDescription] 
KEY INDEX [PK_ProductDescription_ProductDescriptionID] ON ([AW2008FullTextCatalog])
WITH (CHANGE_TRACKING AUTO);
GO

-- check if full text install
SELECT SERVERPROPERTY('IsFullTextInstalled')
