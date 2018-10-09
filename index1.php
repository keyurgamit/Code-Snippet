<html>
<head>    
    <title>Homepage</title>
</head>
 
<body>

    <a href="add.php">Add New Data</a><br/><br/>
 
    <table width='80%' border=0>
        <tr bgcolor='#CCCCCC'>
            <td>ID</td>
		    <td>LastName</td>
            <td>FirstName</td>
            <td>Address</td>
            <td>City</td>
			<td>Action</td>
        </tr>
<?php
//including the database connection file
include("conn.php");
 
//fetching data in descending order (lastest entry first)
//$result = mysql_query("SELECT * FROM users ORDER BY id DESC"); // mysql_query is deprecated
$res = mysql_query("SELECT * FROM persons"); // using mysqli_query instead
while($result = mysql_fetch_array($res))
 {
?>
		<tr>
		<td> <?php echo ($result['ID']); ?></td>
		<td> <?php echo ($result['LastName']); ?></td>
		<td> <?php echo ($result['FirstName']); ?></td>
		<td> <?php echo ($result['Address']); ?></td>
		<td> <?php echo ($result['City']); ?></td>
		<td><a href="del.php?udid=<?php echo($result['ID']); ?> ">Delete</a></td>
		</tr>   
        <?php
		}
        ?>
    </table>
</body>
</html>