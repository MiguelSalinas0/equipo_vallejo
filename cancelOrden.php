<?php

include('soapDebug.php');

$serverName = "10.0.0.114";
$uid = "sa";
$pwd = "Fixl5my38!";
$databaseName = "w1_pueblo";

$connectionInfo = array(
  "UID" => $uid,
  "PWD" => $pwd,
  "Database" => $databaseName
);
$conn = sqlsrv_connect($serverName, $connectionInfo);

if ($conn) {
  echo "Conexión establecida.<br />";
} else {
  echo "Conexión no se pudo establecer.<br />";
  die(print_r(sqlsrv_errors(), true));
}

require_once(__DIR__ . '/vendor/autoload.php');

$grant_type = 'refresh_token';
$user_id = 209369664;
$client_id = '6615910753730134'; // Your client_id
$client_secret = 'h91zzL5wAWg6YMUWF5V2W5BzzeaYLHj4'; // Your client_secret
$redirect_uri = 'https://mercadolibre.com.ar'; // Your redirect_uri

$sql = "SELECT token, retoken FROM meli WHERE id = 2";
$stmt = sqlsrv_query($conn, $sql);
if ($stmt === false) {
  die(print_r(sqlsrv_errors(), true));
} else {
  $ss = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
  //echo($ss["token"]);
}
$to =  date('Y-m-d', strtotime("+1 days")) . 'T00:00:00';
$from = date('Y-m-d', strtotime("-3 days")) . 'T00:00:00';


$url = 'https://api.mercadolibre.com/orders/search?seller=' . $user_id . '&order.status=cancelled&order.date_created.from=' . $from . '.000-00:00&order.date_created.to=' . $to . '.000-00:00';

$headers = [
  'Authorization: Bearer ' . $ss["token"]
];

$metodo = "GET";
$ch = curl_init();
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
if ($metodo == 'GET') {
  curl_setopt($ch, CURLOPT_URL, $url);
} else {
  curl_setopt($ch, CURLOPT_URL, $url);
  curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($datos));
}
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
$tmp = curl_exec($ch);
//print_r($tmp);
$k = json_decode($tmp);
curl_close($ch);


foreach ($k->results as $key) {
  cancelOrden($key);
}


$serverName = "10.0.0.114";
$uid = "sa";
$pwd = "Fixl5my38!";
$databaseName = "w1_pueblo";

$connectionInfo = array(
  "UID" => $uid,
  "PWD" => $pwd,
  "Database" => $databaseName
);
$conn = sqlsrv_connect($serverName, $connectionInfo);

if ($conn) {
  echo "Conexión establecida.<br />";
} else {
  echo "Conexión no se pudo establecer.<br />";
  die(print_r(sqlsrv_errors(), true));
}

require_once(__DIR__ . '/vendor/autoload.php');

$grant_type = 'refresh_token';
$user_id = 257128833;
$client_id = '5038796649356510'; // Your client_id
$client_secret = 'SSP5q4xPndDRru0ut4FelVTg5BrUXRu7'; // Your client_secret
$redirect_uri = 'https://mercadolibres.com.ar'; // Your redirect_uri

$sql = "SELECT token, retoken FROM meli WHERE id = 1";
$stmt = sqlsrv_query($conn, $sql);
if ($stmt === false) {
  die(print_r(sqlsrv_errors(), true));
} else {
  $ss = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
  //echo($ss["token"]);
}


$url = 'https://api.mercadolibre.com/orders/search?seller=' . $user_id . '&order.status=cancelled&order.date_created.from=' . $from . '.000-00:00&order.date_created.to=' . $to . '.000-00:00';

$headers = [
  'Authorization: Bearer ' . $ss["token"]
];

$metodo = "GET";
$ch = curl_init();
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
if ($metodo == 'GET') {
  curl_setopt($ch, CURLOPT_URL, $url);
} else {
  curl_setopt($ch, CURLOPT_URL, $url);
  curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($datos));
}
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
$tmp = curl_exec($ch);
//print_r($tmp);
$k = json_decode($tmp);
curl_close($ch);


foreach ($k->results as $key) {
  cancelOrden($key);
}



function cancelOrden($k2)
{


  $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/SaleDocumentService.svc";

  $log = 'VAPRODC\\matias';
  //echo $log;
  $client = new SoapClient(
    $urlCE . "?singleWsdl",
    array(
      "location" => $urlCE,
      "login" => $log,
      "password" => "MATIAS2020",
      "trace" => true
    )
  );

  $request = new StdClass();
  $request->cancelRequest = new StdClass();

  $request->cancelRequest->Identifier = new StdClass();
  $request->cancelRequest->Identifier->Reference = new StdClass();
  $request->cancelRequest->Identifier->Reference->CustomerId = $k2->buyer->billing_info->doc_number;
  $request->cancelRequest->Identifier->Reference->InternalReference = $k2->id;
  $request->cancelRequest->Identifier->Reference->Type = 'CustomerOrder';
  $request->cancelRequest->ReasonId = "CAN";

  $request->clientContext = new StdClass();
  $request->clientContext->DatabaseId = "VAPRODC";

  try {
    echo "<br>";
    echo "<br>";
    $resu = $client->Cancel($request);
  } catch (Exception $e) {
  }
  soapDebug($client);

  echo "<br>";
  echo "<br>";
}
