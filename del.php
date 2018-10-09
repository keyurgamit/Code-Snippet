<?php
include("conn.php");
$i=$_REQUEST['udid'];
$dquery="delete from persons where ID=".$i."";
mysql_query($dquery);
header("location:index1.php");
?>