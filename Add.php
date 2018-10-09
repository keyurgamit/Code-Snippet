<html>
<head>
    <title>Add Data</title>
</head>
 
<body>

<?php
//including the database connection file
include("conn.php");
 
if(isset($_POST['Insert'])) {    
    $FirstName = $_POST['FirstName'];
	$LastName = $_POST['LastName'];
    $Address = $_POST['Address'];
    $City = $_POST['City'];
        
        // if all the fields are filled (not empty)             
        //insert data to database
        $result = mysql_query("INSERT INTO persons(FirstName,LastName,Address,City) VALUES('$FirstName','$LastName','$Address','$City')");
        
        //display success message
       if($result)
	   {
	    echo "<font color='green'>Data added successfully.";
        }
		else
		{
		echo "Something wrong..";
		}
		echo "<br/><a href='index1.php'>View Result</a>";
    }
?>

    <a href="index1.php">Home</a>
    <br/><br/>
 
    <form action="add.php" method="post" name="form1">
        <table width="25%" border="0">
            <tr> 
                <td>FirstName</td>
                <td><input type="text" name="FirstName"></td>
            </tr>
            <tr> 
                <td>LastName</td>
                <td><input type="text" name="LastName"></td>
            </tr>
            <tr> 
                <td>Address</td>
                <td><input type="text" name="Address"></td>
            </tr>
			  <tr> 
                <td>City</td>
                <td><input type="text" name="City"></td>
            </tr>
            <tr> 
                <td></td>
                <td><input type="submit" name="Insert" value="Insert Data"></td>
            </tr>
        </table>
    </form>
</body>
</html>