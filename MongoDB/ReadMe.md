<h3>Grid to MongoDb POC</h3>

<h4>Purpose</h4>
<p>This solution demonstrates the use of the grid compononent to build a schema in MongoDB.</p>

<h4>Initial setup requirements</h4>
<p>Telerik Kendo UI grid component</p>
MongoDB
https://www.mongodb.com/docs/manual/installation/


<h3>Controllers</h3>
<p>There are 2 controllers, DataController and SchemaController. This demo focuses on the SchemaController.</p>

<h4>Completed functionality</h4>
<p>The crud actions add and delete have been implemented, though see point 2&3 below to be refined.</p>

<h4>Work in progress</h4>
<p>These are areas of further progress required.</p>
1. Rename field, edit field type, edit discription need completing.

2. Onclick delete/rename/event, identifying the column name to pass clicked needs further investigation

3. Identification of the first record for the post api call, depends on an arbitary field "ProductId" from the example. This idealy needs to be on the _id field or equivalent.
line 254 was the beginnings of this approach.

4. Post update the Kendo grid needs to be destroyed and reloaded to prevent grid freezing.


<h3>Further enhancements</h3>
In order to update all records at a later date, I have added placeholder called AddUpdateToMessageQueue. 
This should run out of hours and trigger the api calls with AllRecords flag set to true.


