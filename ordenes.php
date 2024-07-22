<?php

include('soapDebug.php');

// $serverName = "10.0.0.114"; 
//         $uid = "sa";
//         $pwd = "Fixl5my38!";
//         $databaseName = "w1_pueblo"; 

//         $connectionInfo = array( "UID"=>$uid,                            
//                                          "PWD"=>$pwd,                            
//                                          "Database"=>$databaseName);                 
//         $conn = sqlsrv_connect( $serverName, $connectionInfo);



//         $databaseName2 = "db_Cegid"; 
//         $connectionInfo2 = array("UID"=>$uid,
//                                 "PWD"=>$pwd,                            
//                                 "Database"=>$databaseName2);                 
//         $conn2 = sqlsrv_connect( $serverName2, $connectionInfo2); 


$serverName = "10.0.0.114";
$uid = "sa";
$pwd = "Fixl5my38!";
$databaseName1 = "w1_pueblo";
$databaseName2 = "db_Cegid";

$connectionInfo1 = array(
    "UID" => $uid,
    "PWD" => $pwd,
    "Database" => $databaseName1
);

$connectionInfo2 = array(
    "UID" => $uid,
    "PWD" => $pwd,
    "Database" => $databaseName2
);

$conn = sqlsrv_connect($serverName, $connectionInfo1);
$conn2 = sqlsrv_connect($serverName, $connectionInfo2);




if( $conn2 ) {
     echo "Conexión establecida.<br />";
}else{
     echo "Conexión no se pudo establecer.<br />";
     die( print_r( sqlsrv_errors(), true));
}

require_once(__DIR__ . '/vendor/autoload.php');

$grant_type = 'refresh_token';
$user_id = 257128833;
$client_id = '5038796649356510'; // Your client_id
$client_secret = 'SSP5q4xPndDRru0ut4FelVTg5BrUXRu7'; // Your client_secret
$redirect_uri = 'https://mercadolibres.com.ar'; // Your redirect_uri

        $sql = "SELECT token, retoken FROM meli WHERE id = 1";
                $stmt = sqlsrv_query( $conn, $sql);
            if( $stmt === false) {
                die( print_r( sqlsrv_errors(), true) );
            }else {
                $ss = sqlsrv_fetch_array( $stmt, SQLSRV_FETCH_ASSOC);
                //echo($ss["token"]);
            }

    $to =  date('Y-m-d',strtotime("+1 days")).'T00:00:00';
    $from = date('Y-m-d',strtotime("-2 days")).'T00:00:00';



$url = 'https://api.mercadolibre.com/orders/search?seller='.$user_id.'&order.status=paid&order.date_created.from='.$from.'.000-00:00&order.date_created.to='.$to.'.000-00:00';

echo $url. "--------------------------";
   $headers = [    
     'Authorization: Bearer '.$ss["token"]
   ];

   $metodo = "GET"; 
   $ch = curl_init();
   curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
  if($metodo == 'GET') {      
      curl_setopt($ch,CURLOPT_URL,$url);
   } else {
      curl_setopt($ch,CURLOPT_URL,$url);
      curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
   }
   curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
   $tmp = curl_exec($ch);
      //print_r($tmp);
   $k = json_decode($tmp);
   curl_close($ch);
   print_r($k);

  $shipId = 0;
/*
   foreach ($k->results as $key) {

  // $key->cart = 0;

  $url = 'https://api.mercadolibre.com/shipments/'.$key->shipping->id;

   $headers = [    
     'Authorization: Bearer '.$ss["token"]
   ];

   $metodo = "GET"; 
   $ch = curl_init();
   curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
  if($metodo == 'GET') {      
      curl_setopt($ch,CURLOPT_URL,$url);
   } else {
      curl_setopt($ch,CURLOPT_URL,$url);
      curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
   }
   curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
   $tmp = curl_exec($ch);
      //print_r($tmp);
   $kship = json_decode($tmp);
   curl_close($ch);

    $urlb = 'https://api.mercadolibre.com/orders/'.$key->id.'/billing_info';

   $headers = [    
     'Authorization: Bearer '.$ss["token"]
   ];

   $metodo = "GET"; 
   $ch = curl_init();
   curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
  if($metodo == 'GET') {      
      curl_setopt($ch,CURLOPT_URL,$urlb);
   } else {
      curl_setopt($ch,CURLOPT_URL,$urlb);
      curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
   }
   curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
   $tmp = curl_exec($ch);
      //print_r($tmp);
   $ka = json_decode($tmp);
   curl_close($ch);
 // var_dump($ka->billing_info->doc_number);
$key->buyer->address='';
$key->buyer->dni =$ka->billing_info->doc_number;
//$key->buyer->last_name= 
  $key->buyer->last_name= $kship->receiver_address->receiver_name;
   foreach($ka->billing_info->additional_info as $adi){  
    if($adi->type == 'FIRST_NAME')
      $key->buyer->first_name = $adi->value;
   if($adi->type == 'BUSINESS_NAME')
      $key->buyer->last_name = $adi->value;
    if($adi->type == 'LAST_NAME')
      $key->buyer->last_name = $adi->value;
    if($adi->type == 'ZIP_CODE')
      $key->buyer->zip_code = $adi->value;
    if($adi->type == 'STATE_NAME')
       $key->buyer->state = $adi->value;
     if($adi->type == 'STREET_NAME')
       $key->buyer->address .= $adi->value.' ';
     if($adi->type == 'STREET_NUMBER')
       $key->buyer->address .= $adi->value.' ';
      if($adi->type == 'DOC_NUMBER')
       $key->buyer->dni = $adi->value;
    }
   $key->buyer->billing_info = $ka->billing_info;

  if(isset($kship->error) && $kship->error == 'resource not found'){
      $kship->logistic_type = 'cross_docking';
      $key->shipping->id = $key->id."66";
      $kship->receiver_address = 'Acuerdo con el comprador';
    }
  $key->logistic_type = $kship->logistic_type;
      $key->receiver_address = $kship->receiver_address;
   print_r($key);echo "<br /><br />";
   // if($kship->logistic_type == 'cross_docking' || ($kship->logistic_type=='fulfillment' && $kship->status == 'delivered')){


    if($shipId != $key->shipping->id ){

        if(in_array('pack_order', $key->tags)){

            $url = 'https://api.mercadolibre.com/shipments/'.$key->shipping->id.'/items';

             $headers = [    
               'Authorization: Bearer '.$ss["token"]
             ];

             $metodo = "GET"; 
             $ch = curl_init();
             curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
            if($metodo == 'GET') {      
                curl_setopt($ch,CURLOPT_URL,$url);
             } else {
                curl_setopt($ch,CURLOPT_URL,$url);
                curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
             }
             curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
             $tmp = curl_exec($ch);
                //print_r($tmp);
             $ks = json_decode($tmp);
             curl_close($ch);

             if(count($ks)>1){

                    // $key->cart = 1;
                $key->total_amount =0;
                  for ($j=0; $j < count($ks); $j++) { 
                    
                   $oid = $ks[$j]->order_id;

                   $urlin = 'https://api.mercadolibre.com/orders/'.$oid;

                     $headers = [    
                       'Authorization: Bearer '.$ss["token"]
                     ];

                     $metodo = "GET"; 
                     $chin = curl_init();
                     curl_setopt($chin, CURLOPT_HTTPHEADER, $headers);
                    if($metodo == 'GET') {      
                        curl_setopt($chin,CURLOPT_URL,$urlin);
                     } else {
                        curl_setopt($chin,CURLOPT_URL,$urlin);
                        curl_setopt($chin,CURLOPT_POSTFIELDS, json_encode($datos));
                     }
                     curl_setopt($chin,CURLOPT_RETURNTRANSFER,true);
                     $tmpin = curl_exec($chin);
                        //print_r($tmp);
                     $kin = json_decode($tmpin);
                     curl_close($chin);

                     
                     $key->order_items[$j] = $kin->order_items[0];
                         $key->total_amount += $kin->total_amount;
                  }
                    $key->verify_id = $key->id;
                    $key->id = $key->pack_id;

                   // Consultar si existe el cliente en cegid


                            $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";
                            $log = 'VATEST\\matias';

                            $client = new SoapClient( $urlCE . "?singleWsdl",
                                array(
                                "location" => $urlCE,
                                "login" => $log,
                                "password" => "MATIAS2020"
                                )
                            );
                            $request = new StdClass();

                            $request->clientContext = new StdClass();
                            $request->clientContext->DatabaseId = "VATEST";
                            $request->searchData = new StdClass();
                            $request->searchData->FiscalId = $key->buyer->dni;

                            $resu = $client->SearchCustomerIds($request);

                            if(isset($resu->SearchCustomerIdsResult->CustomerQueryData)){
                                // Existe ir a metodo crearOrden

                                crearOrden($key);
                            }
                            else{
                                // No existe crear el cliente y luego metodo crearOrden

                                $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";          
                                $log = 'VATEST\\MATIAS';        
                                $client = new SoapClient( $urlCE . "?singleWsdl",
                                    array(
                                    "location" => $urlCE,
                                    "login" => $log,
                                    "password" => "MATIAS2020",
                                    "trace" => true
                                    )
                                );

                                $request = new StdClass();

                                $request->clientContext = new StdClass();
                                $request->clientContext->DatabaseId = "VATEST";

                                $request->customerData = new StdClass();
                                $request->customerData->CustomerId = $key->buyer->dni;
                                $request->customerData->FirstName = isset($key->buyer->first_name) ? $key->buyer->first_name : 'NN';
                                $request->customerData->IsCompany = false;
                                $request->customerData->LastName = $key->buyer->last_name;
                                $request->customerData->AddressData  = new StdClass();
                                $request->customerData->AddressData->AddressLine1 = $key->buyer->address;
                                $request->customerData->AddressData->City = $key->buyer->state;
                                $request->customerData->AddressData->CountryId = 'ARS';
                                $request->customerData->AddressData->CountryIdType = 'Internal';
                                $request->customerData->AddressData->ZipCode = $key->buyer->zip_code;
                                $request->customerData->PhoneData = new StdClass();
                                $request->customerData->PhoneData->HomePhoneNumber = $key->buyer->phone->number;

                                $request->customerData->UsualStoreId  = '000001';
                                $request->customerData->FiscalId  = $key->buyer->dni;
                                if($key->buyer->billing_info->doc_type == 'CUIT'){
                                   $request->customerData->FirstName = '';
                                  $request->customerData->LastName = $key->buyer->first_name.$key->buyer->last_name;
                                  $request->customerData->CompanyIdNumber = $key->buyer->dni;
                                  $request->customerData->IsCompany = true;
                                }
                                $request->customerData->VATSystem = 'TAX';

                                try {
                                     $resu = $client->AddNewCustomer($request);
                                     echo "<br>";echo "<br>";
                                     print_r( $resu );
                                     crearOrden($key);             
                                  } catch (Exception $e) {}
                                  soapDebug($client);           
                                
                            }




             }elseif(count($ks)==1){

              // Consultar si existe el cliente en cegid
if(isset($key->pack_id))
                    $key->id = $key->pack_id;

                            $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";
                            $log = 'VATEST\\matias';

                            $client = new SoapClient( $urlCE . "?singleWsdl",
                                array(
                                "location" => $urlCE,
                                "login" => $log,
                                "password" => "MATIAS2020"
                                )
                            );
                            $request = new StdClass();

                            $request->clientContext = new StdClass();
                            $request->clientContext->DatabaseId = "VATEST";
                            $request->searchData = new StdClass();
                            $request->searchData->FiscalId = $key->buyer->dni;

                            $resu = $client->SearchCustomerIds($request);

                            if(isset($resu->SearchCustomerIdsResult->CustomerQueryData)){
                                // Existe ir a metodo crearOrden

                                crearOrden($key);
                            }
                            else{
                                // No existe crear el cliente y luego metodo crearOrden

                                $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";          
                                $log = 'VATEST\\MATIAS';        
                                $client = new SoapClient( $urlCE . "?singleWsdl",
                                    array(
                                    "location" => $urlCE,
                                    "login" => $log,
                                    "password" => "MATIAS2020",
                                    "trace" => true
                                    )
                                );

                                $request = new StdClass();

                                $request->clientContext = new StdClass();
                                $request->clientContext->DatabaseId = "VATEST";

                                $request->customerData = new StdClass();
                                $request->customerData->CustomerId = $key->buyer->dni;
                                $request->customerData->FirstName = isset($key->buyer->first_name) ? $key->buyer->first_name : 'NN';
                                $request->customerData->IsCompany = false;
                                $request->customerData->LastName = $key->buyer->last_name;
                                $request->customerData->AddressData  = new StdClass();
                                $request->customerData->AddressData->AddressLine1 = $key->buyer->address;
                                $request->customerData->AddressData->City = $key->buyer->state;
                                $request->customerData->AddressData->CountryId = 'ARS';
                                $request->customerData->AddressData->CountryIdType = 'Internal';
                                $request->customerData->AddressData->ZipCode = $key->buyer->zip_code;
                                $request->customerData->PhoneData = new StdClass();
                                $request->customerData->PhoneData->HomePhoneNumber = $key->buyer->phone->number;

                                $request->customerData->UsualStoreId  = '000001';
                                $request->customerData->FiscalId  = $key->buyer->dni;
                                if($key->buyer->billing_info->doc_type == 'CUIT'){
                                  $request->customerData->FirstName = '';
                                  $request->customerData->LastName = $key->buyer->first_name.$key->buyer->last_name;
                                  $request->customerData->CompanyIdNumber = $key->buyer->dni;
                                  $request->customerData->IsCompany = true;

                                }
                                $request->customerData->VATSystem = 'TAX';

                                try {
                                     $resu = $client->AddNewCustomer($request);
                                     echo "<br>";echo "<br>";
                                     print_r( $resu );
                                     crearOrden($key);             
                                  } catch (Exception $e) {}
                                  soapDebug($client);           
                                
                            }


             }



        }else{
          // Consultar si existe el cliente en cegid


                            $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";
                            $log = 'VATEST\\matias';

                            $client = new SoapClient( $urlCE . "?singleWsdl",
                                array(
                                "location" => $urlCE,
                                "login" => $log,
                                "password" => "MATIAS2020"
                                )
                            );
                            $request = new StdClass();

                            $request->clientContext = new StdClass();
                            $request->clientContext->DatabaseId = "VATEST";
                            $request->searchData = new StdClass();
                            $request->searchData->FiscalId = $key->buyer->dni;

                            $resu = $client->SearchCustomerIds($request);

                            if(isset($resu->SearchCustomerIdsResult->CustomerQueryData)){
                                // Existe ir a metodo crearOrden

                                crearOrden($key);
                            }
                            else{
                                // No existe crear el cliente y luego metodo crearOrden

                                $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";          
                                $log = 'VATEST\\MATIAS';        
                                $client = new SoapClient( $urlCE . "?singleWsdl",
                                    array(
                                    "location" => $urlCE,
                                    "login" => $log,
                                    "password" => "MATIAS2020",
                                    "trace" => true
                                    )
                                );

                                $request = new StdClass();

                                $request->clientContext = new StdClass();
                                $request->clientContext->DatabaseId = "VATEST";

                                $request->customerData = new StdClass();
                                $request->customerData->CustomerId = $key->buyer->dni;
                                $request->customerData->FirstName = $key->buyer->first_name;
                                $request->customerData->IsCompany = false;
                                $request->customerData->LastName = $key->buyer->last_name;
                                $request->customerData->AddressData  = new StdClass();
                                $request->customerData->AddressData->AddressLine1 = $key->buyer->address;
                                $request->customerData->AddressData->City = $key->buyer->state;
                                $request->customerData->AddressData->CountryId = 'ARS';
                                $request->customerData->AddressData->CountryIdType = 'Internal';
                                $request->customerData->AddressData->ZipCode = $key->buyer->zip_code;
                                $request->customerData->PhoneData = new StdClass();
                                $request->customerData->PhoneData->HomePhoneNumber = $key->buyer->phone->number;

                                $request->customerData->UsualStoreId  = '000001';
                                $request->customerData->FiscalId  = $key->buyer->dni;
                                if($key->buyer->billing_info->doc_type == 'CUIT'){
                                  $request->customerData->FirstName = '';
                                  $request->customerData->LastName = $key->buyer->first_name.$key->buyer->last_name;
                                  $request->customerData->CompanyIdNumber = $key->buyer->dni;
                                  $request->customerData->IsCompany = true;
                                }
                                $request->customerData->VATSystem = 'TAX';

                                try {
                                     $resu = $client->AddNewCustomer($request);
                                     echo "<br>";echo "<br>";
                                     print_r( $resu );
                                     crearOrden($key);             
                                  } catch (Exception $e) {}
                                  soapDebug($client);           
                                
                            }


        }

    }

    $shipId = $key->shipping->id;

  // }//llment
 
    }//foreach key


   $cou = round(($k->paging->total / $k->paging->limit), 0, PHP_ROUND_HALF_UP);

   //print_r($k->paging);

  for($i=1; $i < $cou+1; $i++){
    $offset = $i*$k->paging->limit;


  
$url = 'https://api.mercadolibre.com/orders/search?seller='.$user_id.'&order.status=paid&offset='.$offset.'&order.date_created.from='.$from.'.000-00:00&order.date_created.to='.$to.'.000-00:00';

   $headers = [    
     'Authorization: Bearer '.$ss["token"]
   ];

   $metodo = "GET"; 
   $ch = curl_init();
   curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
  if($metodo == 'GET') {      
      curl_setopt($ch,CURLOPT_URL,$url);
   } else {
      curl_setopt($ch,CURLOPT_URL,$url);
      curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
   }
   curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
   $tmp = curl_exec($ch);
      //print_r($tmp);
   $k = json_decode($tmp);
   curl_close($ch);
  // var_dump($k->results);
    $shipId = 0;

   foreach ($k->results as $key) {

    // $key->cart = 0;
$url = 'https://api.mercadolibre.com/shipments/'.$key->shipping->id;

   $headers = [    
     'Authorization: Bearer '.$ss["token"]
   ];

   $metodo = "GET"; 
   $ch = curl_init();
   curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
  if($metodo == 'GET') {      
      curl_setopt($ch,CURLOPT_URL,$url);
   } else {
      curl_setopt($ch,CURLOPT_URL,$url);
      curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
   }
   curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
   $tmp = curl_exec($ch);
      //print_r($tmp);
   $kship = json_decode($tmp);
   curl_close($ch);

    $urlb = 'https://api.mercadolibre.com/orders/'.$key->id.'/billing_info';

   $headers = [    
     'Authorization: Bearer '.$ss["token"]
   ];

   $metodo = "GET"; 
   $ch = curl_init();
   curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
  if($metodo == 'GET') {      
      curl_setopt($ch,CURLOPT_URL,$urlb);
   } else {
      curl_setopt($ch,CURLOPT_URL,$urlb);
      curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
   }
   curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
   $tmp = curl_exec($ch);
      //print_r($tmp);
   $ka = json_decode($tmp);
   curl_close($ch);
 // var_dump($ka->billing_info->doc_number);
$key->buyer->address='';
$key->buyer->dni =$ka->billing_info->doc_number;
//$key->buyer->last_name= 
  $key->buyer->last_name= $kship->receiver_address->receiver_name;
   foreach($ka->billing_info->additional_info as $adi){  
    if($adi->type == 'FIRST_NAME')
      $key->buyer->first_name = $adi->value;
     if($adi->type == 'BUSINESS_NAME')
      $key->buyer->last_name = $adi->value;
    if($adi->type == 'LAST_NAME')
      $key->buyer->last_name = $adi->value;
    if($adi->type == 'ZIP_CODE')
      $key->buyer->zip_code = $adi->value;
    if($adi->type == 'STATE_NAME')
       $key->buyer->state = $adi->value;
     if($adi->type == 'STREET_NAME')
       $key->buyer->address .= $adi->value.' ';
     if($adi->type == 'STREET_NUMBER')
       $key->buyer->address .= $adi->value.' ';
      if($adi->type == 'DOC_NUMBER')
       $key->buyer->dni = $adi->value;
    }
 $key->buyer->billing_info = $ka->billing_info;

  if(isset($kship->error) && $kship->error == 'resource not found'){
      $kship->logistic_type = 'cross_docking';
      $key->shipping->id = $key->id."66";
      $kship->receiver_address = 'Acuerdo con el comprador';
    }
        $key->logistic_type = $kship->logistic_type;

  
        

      $key->receiver_address = $kship->receiver_address;
   print_r($key);echo "<br /><br />";
   // if($kship->logistic_type == 'cross_docking' || ($kship->logistic_type=='fulfillment' && $kship->status == 'delivered')){

    if($shipId != $key->shipping->id ){

        if(in_array('pack_order', $key->tags)){

            $url = 'https://api.mercadolibre.com/shipments/'.$key->shipping->id.'/items';

             $headers = [    
               'Authorization: Bearer '.$ss["token"]
             ];

             $metodo = "GET"; 
             $ch = curl_init();
             curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
            if($metodo == 'GET') {      
                curl_setopt($ch,CURLOPT_URL,$url);
             } else {
                curl_setopt($ch,CURLOPT_URL,$url);
                curl_setopt($ch,CURLOPT_POSTFIELDS, json_encode($datos));
             }
             curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
             $tmp = curl_exec($ch);
                //print_r($tmp);
             $ks = json_decode($tmp);
             curl_close($ch);

             if(count($ks)>1){

                    // $key->cart = 1;
 $key->total_amount = 0;
                 for ($j=0; $j < count($ks); $j++) { 
                    
                   $oid = $ks[$j]->order_id;

                   $urlin = 'https://api.mercadolibre.com/orders/'.$oid;

                     $headers = [    
                       'Authorization: Bearer '.$ss["token"]
                     ];

                     $metodo = "GET"; 
                     $chin = curl_init();
                     curl_setopt($chin, CURLOPT_HTTPHEADER, $headers);
                    if($metodo == 'GET') {      
                        curl_setopt($chin,CURLOPT_URL,$urlin);
                     } else {
                        curl_setopt($chin,CURLOPT_URL,$urlin);
                        curl_setopt($chin,CURLOPT_POSTFIELDS, json_encode($datos));
                     }
                     curl_setopt($chin,CURLOPT_RETURNTRANSFER,true);
                     $tmpin = curl_exec($chin);
                        //print_r($tmp);
                     $kin = json_decode($tmpin);
                     curl_close($chin);

                     
                     $key->order_items[$j] = $kin->order_items[0];
                     $key->total_amount += $kin->total_amount;
                  }
                    $key->verify_id = $key->id;
                    $key->id = $key->pack_id;

                   // Consultar si existe el cliente en cegid


                            $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";
                            $log = 'VATEST\\matias';

                            $client = new SoapClient( $urlCE . "?singleWsdl",
                                array(
                                "location" => $urlCE,
                                "login" => $log,
                                "password" => "MATIAS2020"
                                )
                            );
                            $request = new StdClass();

                            $request->clientContext = new StdClass();
                            $request->clientContext->DatabaseId = "VATEST";
                            $request->searchData = new StdClass();
                            $request->searchData->FiscalId = $key->buyer->dni;

                            $resu = $client->SearchCustomerIds($request);

                            if(isset($resu->SearchCustomerIdsResult->CustomerQueryData)){
                                // Existe ir a metodo crearOrden

                                crearOrden($key);
                            }
                            else{
                                // No existe crear el cliente y luego metodo crearOrden

                                $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";          
                                $log = 'VATEST\\MATIAS';        
                                $client = new SoapClient( $urlCE . "?singleWsdl",
                                    array(
                                    "location" => $urlCE,
                                    "login" => $log,
                                    "password" => "MATIAS2020",
                                    "trace" => true
                                    )
                                );

                                $request = new StdClass();

                                $request->clientContext = new StdClass();
                                $request->clientContext->DatabaseId = "VATEST";

                                $request->customerData = new StdClass();
                                $request->customerData->CustomerId = $key->buyer->dni;
                                $request->customerData->FirstName = $key->buyer->first_name;
                                $request->customerData->IsCompany = false;
                                $request->customerData->LastName = $key->buyer->last_name;
                                $request->customerData->AddressData  = new StdClass();
                                $request->customerData->AddressData->AddressLine1 = $key->buyer->address;
                                $request->customerData->AddressData->City = $key->buyer->state;
                                $request->customerData->AddressData->CountryId = 'ARS';
                                $request->customerData->AddressData->CountryIdType = 'Internal';
                                $request->customerData->AddressData->ZipCode = $key->buyer->zip_code;
                                $request->customerData->PhoneData = new StdClass();
                                $request->customerData->PhoneData->HomePhoneNumber = $key->buyer->phone->number;

                                $request->customerData->UsualStoreId  = '000001';
                                $request->customerData->FiscalId  = $key->buyer->dni;
                                if($key->buyer->billing_info->doc_type == 'CUIT'){
                                  $request->customerData->FirstName = '';
                                  $request->customerData->LastName = $key->buyer->first_name.$key->buyer->last_name;
                                  $request->customerData->CompanyIdNumber = $key->buyer->dni;
                                  $request->customerData->IsCompany = true;
                                }
                                $request->customerData->VATSystem = 'TAX';

                                try {
                                     $resu = $client->AddNewCustomer($request);
                                     echo "<br>";echo "<br>";
                                     print_r( $resu );
                                     crearOrden($key);             
                                  } catch (Exception $e) {}
                                  soapDebug($client);           
                                
                            }




             }elseif(count($ks)==1){

              // Consultar si existe el cliente en cegid
if(isset($key->pack_id))
                    $key->id = $key->pack_id;

                            $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";
                            $log = 'VATEST\\matias';

                            $client = new SoapClient( $urlCE . "?singleWsdl",
                                array(
                                "location" => $urlCE,
                                "login" => $log,
                                "password" => "MATIAS2020"
                                )
                            );
                            $request = new StdClass();

                            $request->clientContext = new StdClass();
                            $request->clientContext->DatabaseId = "VATEST";
                            $request->searchData = new StdClass();
                            $request->searchData->FiscalId = $key->buyer->dni;

                            $resu = $client->SearchCustomerIds($request);

                            if(isset($resu->SearchCustomerIdsResult->CustomerQueryData)){
                                // Existe ir a metodo crearOrden

                                crearOrden($key);
                            }
                            else{
                                // No existe crear el cliente y luego metodo crearOrden

                                $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";          
                                $log = 'VATEST\\MATIAS';        
                                $client = new SoapClient( $urlCE . "?singleWsdl",
                                    array(
                                    "location" => $urlCE,
                                    "login" => $log,
                                    "password" => "MATIAS2020",
                                    "trace" => true
                                    )
                                );

                                $request = new StdClass();

                                $request->clientContext = new StdClass();
                                $request->clientContext->DatabaseId = "VATEST";

                                $request->customerData = new StdClass();
                                $request->customerData->CustomerId = $key->buyer->dni;
                                $request->customerData->FirstName = $key->buyer->first_name;
                                $request->customerData->IsCompany = false;
                                $request->customerData->LastName = $key->buyer->last_name;
                                $request->customerData->AddressData  = new StdClass();
                                $request->customerData->AddressData->AddressLine1 = $key->buyer->address;
                                $request->customerData->AddressData->City = $key->buyer->state;
                                $request->customerData->AddressData->CountryId = 'ARS';
                                $request->customerData->AddressData->CountryIdType = 'Internal';
                                $request->customerData->AddressData->ZipCode = $key->buyer->zip_code;
                                $request->customerData->PhoneData = new StdClass();
                                $request->customerData->PhoneData->HomePhoneNumber = $key->buyer->phone->number;

                                $request->customerData->UsualStoreId  = '000001';
                                $request->customerData->FiscalId  = $key->buyer->dni;
                                if($key->buyer->billing_info->doc_type == 'CUIT'){
                                   $request->customerData->FirstName = '';
                                  $request->customerData->LastName = $key->buyer->first_name.$key->buyer->last_name;
                                  $request->customerData->CompanyIdNumber = $key->buyer->dni;
                                  $request->customerData->IsCompany = true;
                                }
                                $request->customerData->VATSystem = 'TAX';

                                try {
                                     $resu = $client->AddNewCustomer($request);
                                     echo "<br>";echo "<br>";
                                     print_r( $resu );
                                     crearOrden($key);             
                                  } catch (Exception $e) {}
                                  soapDebug($client);           
                                
                            }


             }



        }else{
          // Consultar si existe el cliente en cegid


                            $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";
                            $log = 'VATEST\\matias';

                            $client = new SoapClient( $urlCE . "?singleWsdl",
                                array(
                                "location" => $urlCE,
                                "login" => $log,
                                "password" => "MATIAS2020"
                                )
                            );
                            $request = new StdClass();

                            $request->clientContext = new StdClass();
                            $request->clientContext->DatabaseId = "VATEST";
                            $request->searchData = new StdClass();
                            $request->searchData->FiscalId = $key->buyer->dni;

                            $resu = $client->SearchCustomerIds($request);

                            if(isset($resu->SearchCustomerIdsResult->CustomerQueryData)){
                                // Existe ir a metodo crearOrden

                                crearOrden($key);
                            }
                            else{
                                // No existe crear el cliente y luego metodo crearOrden

                                $urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc";          
                                $log = 'VATEST\\MATIAS';        
                                $client = new SoapClient( $urlCE . "?singleWsdl",
                                    array(
                                    "location" => $urlCE,
                                    "login" => $log,
                                    "password" => "MATIAS2020",
                                    "trace" => true
                                    )
                                );

                                $request = new StdClass();

                                $request->clientContext = new StdClass();
                                $request->clientContext->DatabaseId = "VATEST";

                                $request->customerData = new StdClass();
                                $request->customerData->CustomerId = $key->buyer->dni;
                                $request->customerData->FirstName = $key->buyer->first_name;
                                $request->customerData->IsCompany = false;
                                $request->customerData->LastName = $key->buyer->last_name;
                                $request->customerData->AddressData  = new StdClass();
                                $request->customerData->AddressData->AddressLine1 = $key->buyer->address;
                                $request->customerData->AddressData->City = $key->buyer->state;
                                $request->customerData->AddressData->CountryId = 'ARS';
                                $request->customerData->AddressData->CountryIdType = 'Internal';
                                $request->customerData->AddressData->ZipCode = $key->buyer->zip_code;
                                $request->customerData->PhoneData = new StdClass();
                                $request->customerData->PhoneData->HomePhoneNumber = $key->buyer->phone->number;

                                $request->customerData->UsualStoreId  = '000001';
                                $request->customerData->FiscalId  = $key->buyer->dni;
                                if($key->buyer->billing_info->doc_type == 'CUIT'){
                                  $request->customerData->FirstName = '';
                                  $request->customerData->LastName = $key->buyer->first_name.$key->buyer->last_name;
                                  $request->customerData->CompanyIdNumber = $key->buyer->dni;
                                  $request->customerData->IsCompany = true;
                                }
                                $request->customerData->VATSystem = 'TAX';

                                try {
                                     $resu = $client->AddNewCustomer($request);
                                     echo "<br>";echo "<br>";
                                     print_r( $resu );
                                     crearOrden($key);             
                                  } catch (Exception $e) {}
                                  soapDebug($client);           
                                
                            }


        }

    }

    $shipId = $key->shipping->id;


       // }//fulfillment
    }//foreach
    }//for

    //conexion con CEGID




function crearOrden($k2){

$urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/SaleDocumentService.svc";

$log = 'VATEST\\MATIAS';
//echo $log;
$client = new SoapClient( $urlCE . "?singleWsdl",
    array(
    "location" => $urlCE,
    "login" => $log,
    "password" => "MATIAS2020",
    "trace" => true
    )
);
// CONSULTA SI EXISTE LA ORDEN EN CEGID

// if($k2->id=='2000002788275211')
  //  $k2->id='0'.$k2->id;

// echo "<br><br>entrooooo".$k2->id;

if(isset($_POST['new'])){
    $k2->id = $_POST['new'].$k2->id;
    echo "<br>";
    echo "reemplaza numero de orden por: ". $_POST['new'].$k2->id;
    echo "<br>";
}

$request = new StdClass();

$request->clientContext = new StdClass();
$request->clientContext->DatabaseId = "VATEST";

$request->searchRequest = new StdClass();
$request->searchRequest->Reference = new StdClass();
$request->searchRequest->Reference->CustomerId = $k2->buyer->billing_info->doc_number;
$request->searchRequest->Reference->InternalReference = $k2->id;
$request->searchRequest->Reference->Type = 'CustomerOrder';

try {
    $resu = $client->GetByReference($request);
  } catch (Exception $e) {}
  // //soapDebug($client);

$request->searchRequest = new StdClass();
$request->searchRequest->Reference = new StdClass();
$request->searchRequest->Reference->CustomerId = $k2->buyer->billing_info->doc_number;
$request->searchRequest->Reference->InternalReference = $k2->id;
$request->searchRequest->Reference->Type = 'CustomerOrder';

try {
    $resuVeri = $client->GetByReference($request);
  } catch (Exception $e) {}
  
if(isset($resu->GetByReferenceResult) || isset($resuVeri->GetByReferenceResult)){
echo ' existe';echo "<br>";

}else{

  // NO EXISTE LA ORDEN LA CREA

$urlCE = "http://cegid.sportotal.com.ar/Y2_VAL/SaleDocumentService.svc";

$log = 'VATEST\\MATIAS';
//echo $log;
$client = new SoapClient( $urlCE . "?singleWsdl",
    array(
    "location" => $urlCE,
    "login" => $log,
    "password" => "MATIAS2020",
    "trace" => true
    )
);

$request = new StdClass();

$request2 = new StdClass();

$request->createRequest = new StdClass();

$request2->createRequest = new StdClass();

if(isset($k2->receiver_address)){

$request->createRequest->DeliveryAddress = new StdClass();
$request->createRequest->DeliveryAddress->City = $k2->receiver_address->state->name;
$request->createRequest->DeliveryAddress->CountryId  = 'ARS';
$request->createRequest->DeliveryAddress->CountryIdType = 'Internal';
$request->createRequest->DeliveryAddress->FirstName = $k2->receiver_address->receiver_name;
$request->createRequest->DeliveryAddress->LastName = $k2->receiver_address->receiver_name; //$k2->buyer->last_name;
$request->createRequest->DeliveryAddress->Line1 = $k2->receiver_address->address_line;
$request->createRequest->DeliveryAddress->ZipCode  = $k2->receiver_address->zip_code;

$request2->createRequest->DeliveryAddress = new StdClass();
$request2->createRequest->DeliveryAddress->City = $k2->receiver_address->state->name;
$request2->createRequest->DeliveryAddress->CountryId  = 'ARS';
$request2->createRequest->DeliveryAddress->CountryIdType = 'Internal';
$request2->createRequest->DeliveryAddress->FirstName = $k2->receiver_address->receiver_name;
$request2->createRequest->DeliveryAddress->LastName = $k2->receiver_address->receiver_name; //$k2->buyer->last_name;
$request2->createRequest->DeliveryAddress->Line1 = $k2->receiver_address->address_line;
$request2->createRequest->DeliveryAddress->ZipCode  = $k2->receiver_address->zip_code;

}

$request->createRequest->Header = new StdClass();
$request->createRequest->Header->Active = true;
$request->createRequest->Header->Comment = $k2->shipping->id;
$request->createRequest->Header->CustomerId  = $k2->buyer->billing_info->doc_number;
$request->createRequest->Header->CurrencyId = 'ARG';
$request->createRequest->Header->Date = substr($k2->date_created, 0, 10);

$request->createRequest->Header->InternalReference = $k2->id;

$request->createRequest->Header->OmniChannel = new StdClass();
$request->createRequest->Header->OmniChannel->BillingStatus = 'Pending';

// $request->createRequest->Header->OmniChannel->DeliveryType = 'BookedInStore';
// $request->createRequest->Header->OmniChannel->FollowUpStatus = 'Validated';

$request->createRequest->Header->OmniChannel->DeliveryType = 'ShipByCentral';
$request->createRequest->Header->OmniChannel->FollowUpStatus = 'WaitingCommodity';
$request->createRequest->Header->OmniChannel->GiftMessageType = 'None';
$request->createRequest->Header->OmniChannel->PaymentStatus = 'Totally';
$request->createRequest->Header->OmniChannel->ReturnStatus = 'NotReturned';
$request->createRequest->Header->OmniChannel->ShippingStatus = 'Pending';
$request->createRequest->Header->OmniChannel->Transporter = 'MELI VALLEJO';


$request->createRequest->Header->Origin = 'ECommerce';

if($k2->logistic_type == 'fulfillment'){ 
  // $request->createRequest->Header->OmniChannel->DeliveryStoreId = '000081';
 $request->createRequest->Header->StoreId  = '000081';
$request->createRequest->Header->WarehouseId  = '000081';
  $request->createRequest->Header->SalesPersonId  = 'FULL';
  $request->createRequest->Header->OmniChannel->FollowUpStatus = 'Validated';
}elseif($k2->logistic_type == 'cross_docking'){
  // $request->createRequest->Header->OmniChannel->DeliveryStoreId = '000002';
$request->createRequest->Header->StoreId  = '000102';
$request->createRequest->Header->WarehouseId  = '000102';
// $request->createRequest->Header->OmniChannel->FollowUpStatus = 'Validated';
  $request->createRequest->Header->SalesPersonId  = 'MELI';
 // $Create_Line->OmniChannel->FollowUpStatus = 'RequestedStoreBooking';
}
$request->createRequest->Header->Type  = 'CustomerOrder';
        
$request2->createRequest->Header = new StdClass();
$request2->createRequest->Header->Active = true;
$request2->createRequest->Header->Comment = $k2->shipping->id;
$request2->createRequest->Header->CustomerId  = $k2->buyer->billing_info->doc_number;
$request2->createRequest->Header->CurrencyId = 'ARG';
$request2->createRequest->Header->Date = substr($k2->date_created, 0, 10);

$request2->createRequest->Header->InternalReference = $k2->id.'-SPLIT';

$request2->createRequest->Header->OmniChannel = new StdClass();
$request2->createRequest->Header->OmniChannel->BillingStatus = 'Pending';

// $request->createRequest->Header->OmniChannel->DeliveryType = 'BookedInStore';
// $request->createRequest->Header->OmniChannel->FollowUpStatus = 'Validated';

$request2->createRequest->Header->OmniChannel->DeliveryType = 'ShipByCentral';
$request2->createRequest->Header->OmniChannel->FollowUpStatus = 'WaitingCommodity';
$request2->createRequest->Header->OmniChannel->GiftMessageType = 'None';
$request2->createRequest->Header->OmniChannel->PaymentStatus = 'Totally';
$request2->createRequest->Header->OmniChannel->ReturnStatus = 'NotReturned';
$request2->createRequest->Header->OmniChannel->ShippingStatus = 'Pending';
$request2->createRequest->Header->OmniChannel->Transporter = 'MELI VALLEJO';


$request2->createRequest->Header->Origin = 'ECommerce';

if($k2->logistic_type == 'fulfillment'){ 
  // $request->createRequest->Header->OmniChannel->DeliveryStoreId = '000081';
 $request2->createRequest->Header->StoreId  = '000081';
$request2->createRequest->Header->WarehouseId  = '000081';
  $request2->createRequest->Header->SalesPersonId  = 'FULL';
  $request2->createRequest->Header->OmniChannel->FollowUpStatus = 'Validated';
}elseif($k2->logistic_type == 'cross_docking'){
  // $request->createRequest->Header->OmniChannel->DeliveryStoreId = '000002';
$request2->createRequest->Header->StoreId  = '000102';
$request2->createRequest->Header->WarehouseId  = '000198';
$request2->createRequest->Header->OmniChannel->FollowUpStatus = 'Validated';
// $request->createRequest->Header->OmniChannel->FollowUpStatus = 'Validated';
  $request2->createRequest->Header->SalesPersonId  = 'MELI';
 // $Create_Line->OmniChannel->FollowUpStatus = 'RequestedStoreBooking';
}
$request2->createRequest->Header->Type  = 'CustomerOrder';  

$request->createRequest->Lines = array();
$request2->createRequest->Lines = array();
$resu199=0;
$resu198=0;

<<<<<<< Updated upstream
foreach ($k2->order_items as $items) { 
    $it = $items->item;
  
  /*  if (strlen($it->seller_sku) < 6) {
        $sql1 = "SELECT [skuReferenceCode] FROM [W1_Pueblo].[dbo].[TBL_SKU_CODBARRA] where [skuId] =" . $it->seller_sku;
        $sku = sqlsrv_query($conn, $sql1);
        if ($sku === false) {
            die(print_r(sqlsrv_errors(), true));
        } else {
            $ss = sqlsrv_fetch_array($sku, SQLSRV_FETCH_ASSOC);
        }
        echo "<br>";
        echo "reemplaza sku: " . $it->seller_sku . " por sku: " . $ss['skuReferenceCode'];
        echo "<br>";
        $it->seller_sku = $ss['skuReferenceCode'];
=======
    // foreach ($k2->order_items as $items) {
    //   $it = $items->item;

    //   if (strlen($it->seller_sku) < 6) {
    //     $sql1 = "SELECT [skuReferenceCode] FROM [W1_Pueblo].[dbo].[TBL_SKU_CODBARRA] where [skuId] =" . $it->seller_sku;
    //     $sku = sqlsrv_query($conn, $sql1);
    //     if ($sku === false) {
    //       die(print_r(sqlsrv_errors(), true));
    //     } else {
    //       $ss = sqlsrv_fetch_array($sku, SQLSRV_FETCH_ASSOC);
    //     }
    //     echo "<br>";
    //     echo "reemplaza sku: " . $it->seller_sku . " por sku: " . $ss['skuReferenceCode'];
    //     echo "<br>";
    //     $it->seller_sku = $ss['skuReferenceCode'];
    //   }

    //   if (isset($_POST['badsku']) && isset($_POST['goodsku']) && $_POST['badsku'] !== "" && $_POST['goodsku'] !== "") {
    //     if ($it->seller_sku == $_POST['badsku']) {
    //       $it->seller_sku = $_POST['goodsku'];
    //       echo "<br>";
    //       echo "reemplaza sku: " . $_POST['badsku'] . " por sku: " . $_POST['goodsku'];
    //       echo "<br>";
    //     } else {
    //       echo "<br> El sku incorrecto no es el que venia la orden.<br>";
    //     }
    //   }


      $urlS = "http://cegid.sportotal.com.ar/Y2_VAL/ItemInventoryWcfService.svc";

      $clientS = new SoapClient(
        $urlS . "?singleWsdl",
        array(
          "location" => $urlS,
          "login" => $log,
          "password" => "MATIAS2020",
          "trace" => true
        )
      );

      $requestS = new StdClass();
      $requestS->clientContext = new StdClass();
      $requestS->clientContext->DatabaseId = "VATEST";

      $requestS->itemIdentifier = new StdClass();
      $requestS->itemIdentifier->Reference = $it->seller_sku;
      $requestS->storeId = '000199';
      $resu199 = $clientS->GetAvailableQty($requestS);
      echo ('STOCK DE LA 199: ' . $resu199->GetAvailableQtyResult->AvailableQty);
      if ($resu199->GetAvailableQtyResult->AvailableQty >= $items->quantity) {
        echo ('saco stock de la 199');
        $Create_Line = new StdClass();
        $Create_Line->ItemIdentifier = new StdClass();
        $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

        $Create_Line->Label = $it->title;
        $Create_Line->Origin = 'ECommerce';
        $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
        $Create_Line->Quantity = $items->quantity;
        $Create_Line->NetUnitPrice = $items->unit_price;
        $Create_Line->OmniChannel = new StdClass();
        $Create_Line->OmniChannel->WarehouseId = '000199';
        $Create_Line->SalesPersonId = 'MELI';
        array_push($request->createRequest->Lines, $Create_Line);

        //var_dump($request);
        //$client->Create($request);

        // $r = 0;
      } else {
        $urlS = "http://cegid.sportotal.com.ar/Y2_VAL/ItemInventoryWcfService.svc";

        $clientS = new SoapClient(
          $urlS . "?singleWsdl",
          array(
            "location" => $urlS,
            "login" => $log,
            "password" => "MATIAS2020",
            "trace" => true
          )
        );

        $requestS = new StdClass();
        $requestS->clientContext = new StdClass();
        $requestS->clientContext->DatabaseId = "VATEST";

        $requestS->itemIdentifier = new StdClass();
        $requestS->itemIdentifier->Reference = $it->seller_sku;
        $requestS->storeId = '000198';
        $resu198 = $clientS->GetAvailableQty($requestS);
        echo ('STOCK DE LA 198: ' . $resu198->GetAvailableQtyResult->AvailableQty);
        if ($resu198->GetAvailableQtyResult->AvailableQty >=  $items->quantity) {
          echo ('saco stock de la 198');
          $Create_Line = new StdClass();
          $Create_Line->ItemIdentifier = new StdClass();
          $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

          $Create_Line->Label = $it->title;
          $Create_Line->Origin = 'ECommerce';
          $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
          $Create_Line->Quantity = $items->quantity;
          $Create_Line->NetUnitPrice = $items->unit_price;

          if ($k2->logistic_type == 'cross_docking') {
            $Create_Line->OmniChannel = new StdClass();
            //  $Create_Line->OmniChannel->WarehouseId = '000198';
            $Create_Line->SalesPersonId = 'MELI';
          } elseif ($k2->logistic_type == 'fulfillment') {
            $Create_Line->SalesPersonId = 'FULL';
          }
          array_push($request2->createRequest->Lines, $Create_Line);
        } elseif ($resu198->GetAvailableQtyResult->AvailableQty + $resu199->GetAvailableQtyResult->AvailableQty == $items->quantity) {
          echo ('saco stock de la 199 y de la 198');
          $Create_Line = new StdClass();
          $Create_Line->ItemIdentifier = new StdClass();
          $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

          $Create_Line->Label = $it->title;
          $Create_Line->Origin = 'ECommerce';
          $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
          $Create_Line->Quantity = $resu199->GetAvailableQtyResult->AvailableQty;
          $Create_Line->NetUnitPrice = $items->unit_price;
          $Create_Line->OmniChannel = new StdClass();
          $Create_Line->OmniChannel->WarehouseId = '000199';
          $Create_Line->SalesPersonId = 'MELI';
          array_push($request->createRequest->Lines, $Create_Line);

          $Create_Line = new StdClass();
          $Create_Line->ItemIdentifier = new StdClass();
          $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

          $Create_Line->Label = $it->title;
          $Create_Line->Origin = 'ECommerce';
          $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
          $Create_Line->Quantity = $items->quantity - $resu199->GetAvailableQtyResult->AvailableQty;
          $Create_Line->NetUnitPrice = $items->unit_price;

          if ($k2->logistic_type == 'cross_docking') {
            $Create_Line->OmniChannel = new StdClass();
            //  $Create_Line->OmniChannel->WarehouseId = '000198';
            $Create_Line->SalesPersonId = 'MELI';
          } elseif ($k2->logistic_type == 'fulfillment') {
            $Create_Line->SalesPersonId = 'FULL';
          }
          array_push($request2->createRequest->Lines, $Create_Line);
        } else {
          echo "No hay stock saca todo de la 199";
          $Create_Line = new StdClass();
          $Create_Line->ItemIdentifier = new StdClass();
          $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

          $Create_Line->Label = $it->title;
          $Create_Line->Origin = 'ECommerce';
          $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
          $Create_Line->Quantity = $items->quantity;
          $Create_Line->NetUnitPrice = $items->unit_price;
          $Create_Line->OmniChannel = new StdClass();
          $Create_Line->OmniChannel->WarehouseId = '000199';
          $Create_Line->SalesPersonId = 'MELI';
          array_push($request->createRequest->Lines, $Create_Line);
        }
      }
>>>>>>> Stashed changes
    }

    if (isset($_POST['badsku']) && isset($_POST['goodsku']) && $_POST['badsku'] !== "" && $_POST['goodsku'] !== "") {
        if ($it->seller_sku == $_POST['badsku']) {
            $it->seller_sku = $_POST['goodsku'];
            echo "<br>";
            echo "reemplaza sku: " . $_POST['badsku'] . " por sku: " . $_POST['goodsku'];
            echo "<br>";
        } else {
            echo "<br> El sku incorrecto no es el que venia la orden.<br>";
        }
    }


       $urlS = "http://cegid.sportotal.com.ar/Y2_VAL/ItemInventoryWcfService.svc";
 
    $clientS = new SoapClient( $urlS . "?singleWsdl",
        array(
        "location" => $urlS,
        "login" => $log, 
        "password" => "MATIAS2020",
        "trace" => true
        )
    );

            $requestS = new StdClass();
            $requestS->clientContext = new StdClass();
            $requestS->clientContext->DatabaseId = "VATEST";

            $requestS->itemIdentifier = new StdClass();
            $requestS->itemIdentifier->Reference = $it->seller_sku;
            $requestS->storeId = '000199';
            $resu199 = $clientS->GetAvailableQty($requestS);
            ECHO ('STOCK DE LA 199: '.$resu199->GetAvailableQtyResult->AvailableQty);    
            if($resu199->GetAvailableQtyResult->AvailableQty >= $items->quantity){
            echo('saco stock de la 199');
            $Create_Line = new StdClass();
            $Create_Line->ItemIdentifier = new StdClass();
            $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

                $Create_Line->Label = $it->title;
                $Create_Line->Origin = 'ECommerce';
                $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
                $Create_Line->Quantity = $items->quantity;
                $Create_Line->NetUnitPrice = $items->unit_price;
                $Create_Line->OmniChannel = new StdClass();
                $Create_Line->OmniChannel->WarehouseId = '000199';
                $Create_Line->SalesPersonId = 'MELI';
                array_push($request->createRequest->Lines,$Create_Line);
                
                //var_dump($request);
                //$client->Create($request);

           // $r = 0;
                     } else {
                     $urlS = "http://cegid.sportotal.com.ar/Y2_VAL/ItemInventoryWcfService.svc";

                        $clientS = new SoapClient( $urlS . "?singleWsdl",
                            array(
                            "location" => $urlS,
                            "login" => $log, 
                            "password" => "MATIAS2020",
                            "trace" => true
                            )
                        );

                        $requestS = new StdClass();
                        $requestS->clientContext = new StdClass();
                        $requestS->clientContext->DatabaseId = "VATEST";

                        $requestS->itemIdentifier = new StdClass();
                        $requestS->itemIdentifier->Reference = $it->seller_sku;
                        $requestS->storeId = '000198';   
                    $resu198 = $clientS->GetAvailableQty($requestS);   
                    ECHO ('STOCK DE LA 198: '.$resu198->GetAvailableQtyResult->AvailableQty); 
                    if($resu198->GetAvailableQtyResult->AvailableQty >=  $items->quantity){
                    echo('saco stock de la 198');
                    $Create_Line = new StdClass();
                    $Create_Line->ItemIdentifier = new StdClass();
                    $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

                    $Create_Line->Label = $it->title;
                    $Create_Line->Origin = 'ECommerce';
                    $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
                    $Create_Line->Quantity = $items->quantity;
                    $Create_Line->NetUnitPrice = $items->unit_price;

                    if ($k2->logistic_type == 'cross_docking') {
                        $Create_Line->OmniChannel = new StdClass();
                       //  $Create_Line->OmniChannel->WarehouseId = '000198';
                        $Create_Line->SalesPersonId = 'MELI';
                    } elseif ($k2->logistic_type == 'fulfillment') {
                        $Create_Line->SalesPersonId = 'FULL';
                    }
                  array_push($request2->createRequest->Lines, $Create_Line);
                   }elseif ($resu198->GetAvailableQtyResult->AvailableQty + $resu199->GetAvailableQtyResult->AvailableQty == $items->quantity){
                 echo('saco stock de la 199 y de la 198');
                $Create_Line = new StdClass();
                $Create_Line->ItemIdentifier = new StdClass();
                $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

                $Create_Line->Label = $it->title;
                $Create_Line->Origin = 'ECommerce';
                $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
                $Create_Line->Quantity = $resu199->GetAvailableQtyResult->AvailableQty;
                $Create_Line->NetUnitPrice = $items->unit_price;
                $Create_Line->OmniChannel = new StdClass();
                $Create_Line->OmniChannel->WarehouseId = '000199';
                $Create_Line->SalesPersonId = 'MELI';
                array_push($request->createRequest->Lines,$Create_Line);
              
                    $Create_Line = new StdClass();
                    $Create_Line->ItemIdentifier = new StdClass();
                    $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

                    $Create_Line->Label = $it->title;
                    $Create_Line->Origin = 'ECommerce';
                    $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
                    $Create_Line->Quantity = $items->quantity-$resu199->GetAvailableQtyResult->AvailableQty;
                    $Create_Line->NetUnitPrice = $items->unit_price;

                    if ($k2->logistic_type == 'cross_docking') {
                        $Create_Line->OmniChannel = new StdClass();
                       //  $Create_Line->OmniChannel->WarehouseId = '000198';
                        $Create_Line->SalesPersonId = 'MELI';
                    } elseif ($k2->logistic_type == 'fulfillment') {
                        $Create_Line->SalesPersonId = 'FULL';
                    }
                  array_push($request2->createRequest->Lines, $Create_Line);

                }
                else {
                   /* echo "No hay stock saca todo de la 199";
                    $Create_Line = new StdClass();
                    $Create_Line->ItemIdentifier = new StdClass();
                    $Create_Line->ItemIdentifier->Reference = isset($it->seller_sku) ? $it->seller_sku : $it->seller_custom_field;

                    $Create_Line->Label = $it->title;
                    $Create_Line->Origin = 'ECommerce';
                    $Create_Line->DeliveryDate = substr($k2->date_created, 0, 10);
                    $Create_Line->Quantity = $items->quantity;
                    $Create_Line->NetUnitPrice = $items->unit_price;
                    $Create_Line->OmniChannel = new StdClass();
                    $Create_Line->OmniChannel->WarehouseId = '000199';
                    $Create_Line->SalesPersonId = 'MELI';
                    array_push($request->createRequest->Lines,$Create_Line);
                
                }
                }
                
}

                $request->createRequest->Payments = new StdClass();
                $request->createRequest->Payments->Create_Payment = new StdClass();
                $request->createRequest->Payments->Create_Payment->Amount = 0;
                $request->createRequest->Payments->Create_Payment->MethodId  =  "ECO";
                $request->createRequest->Payments->Create_Payment->Id  = 20;
                $request->createRequest->Payments->Create_Payment->DueDate  = substr($k2->date_created, 0, 10);
                $request->createRequest->Payments->Create_Payment->IsReceivedPayment  = false;
                $request->createRequest->Payments->Create_Payment->CurrencyId = 'ARG';

                $request2->createRequest->Payments = new StdClass();
                $request2->createRequest->Payments->Create_Payment = new StdClass();
                $request2->createRequest->Payments->Create_Payment->Amount = 0;
                $request2->createRequest->Payments->Create_Payment->MethodId  =  "ECO";
                $request2->createRequest->Payments->Create_Payment->Id  = 20;
                $request2->createRequest->Payments->Create_Payment->DueDate  = substr($k2->date_created, 0, 10);
                $request2->createRequest->Payments->Create_Payment->IsReceivedPayment  = false;
                $request2->createRequest->Payments->Create_Payment->CurrencyId = 'ARG';




$request->clientContext = new StdClass();
$request->clientContext->DatabaseId = "VATEST";
$request2->clientContext = new StdClass();
$request2->clientContext->DatabaseId = "VATEST";

 echo "<br>";echo "<br>";

 echo "<br>";echo "<br>";
try {
//$resu = $client->Create($request);
 
      // Verificar si la primera variable no está vacía y hacer un var_dump si contiene datos
if (!empty($request->createRequest->Lines)) {
    $resu = $client->Create($request);
    print_r( $resu );
}

// Verificar si la segunda variable no está vacía y hacer un var_dump si contiene datos
if (!empty($request2->createRequest->Lines)) {
    $resu2 = $client->Create($request2);
    print_r( $resu2 );
}

// Crear la consulta para ejecutar el procedimiento almacenado
$serverName = "10.0.0.115"; 
        $uid = "sa";
        $pwd = "MicroS123";
        $databaseName = "TABLEROS"; 

        $connectionInfo2 = array( "UID"=>$uid,                            
                                         "PWD"=>$pwd,                            
                                         "Database"=>$databaseName);                 
        $con = sqlsrv_connect( $serverName, $connectionInfo2);          

if( $con ) {
     echo "Conexión establecida.<br />";
}else{
     echo "Conexión no se pudo establecer.<br />";
     die( print_r( sqlsrv_errors(), true));
}
// ...

if(isset($k2->cancel_detail)) {
    $cancelReason = $k2->cancel_detail->description;
    $fecha = date_create_from_format('Y-m-d\TH:i:s.uP', $k2->cancel_detail->date);
    $cancellationDate = $fecha->format('Y-m-d H:i:s');
} else {
    $cancelReason = null;
    $cancellationDate = null;
}

$sql = "EXEC INSERTAR_ACTUALIZAR_ORDENES_MELI 
        @orderId='".$k2->id."', 
        @sellerId='MELI VALLEJO', 
        @sellerName='MELI VALLEJO', 
        @status='".$k2->status."', 
        @shippingData_Address_City='".$k2->receiver_address->city->name."', 
        @logisticsInfo_selectedSla='".$k2->logistic_type."', 
        @cancelReason='".$cancelReason."',
        @cancellationDate='".$cancellationDate."'";
// Ejecutar la consulta
$stmt = sqlsrv_query( $con, $sql);

if( $stmt === false) {
    die( print_r( sqlsrv_errors(), true) );
} else {
    echo "El procesamiento almacenado se ha ejecutado correctamente";         
}

        
//$resu2 = $client->Create($request2);
  } catch (Exception $e) {echo "error creando orden<br>"; var_dump($e);}
  //soapDebug($client);

 echo "<br>";echo "<br>";
        }
}
*/
?>