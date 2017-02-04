using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Specialized;


namespace SuiteCRMWebServiceMethodCalls
{
    //PHP cURL codes in this project are copied from http://support.sugarcrm.com/

    public class Class1
    {
        private Uri site_uri;
        private string session;
        private string username;
        private string password;

        public JObject Call(string method, JObject parameters)
        {
            JProperty s = new JProperty("session", session);
            parameters.AddFirst(s);
            
            NameValueCollection body = new NameValueCollection();
            body.Add("method", method);
            body.Add("input_type", "JSON");
            body.Add("output_type", "JSON");
            body.Add("rest_data", JsonConvert.SerializeObject(parameters));

            WebClient client = new WebClient();
            byte[] response = client.UploadValues("site_uri", "POST", body);
            return JObject.Parse(System.Text.Encoding.Default.GetString(response));
        }
        //If we use PHP cURL, "Call" function should look like:

        //function call($method, $parameters, $url)
        //{
        //    ob_start();
        //    $curl_request = curl_init();
        //    curl_setopt($curl_request, CURLOPT_URL, $url);
        //    curl_setopt($curl_request, CURLOPT_POST, 1);
        //    curl_setopt($curl_request, CURLOPT_HTTP_VERSION, CURL_HTTP_VERSION_1_0);
        //    curl_setopt($curl_request, CURLOPT_HEADER, 1);
        //    curl_setopt($curl_request, CURLOPT_SSL_VERIFYPEER, 0);
        //    curl_setopt($curl_request, CURLOPT_RETURNTRANSFER, 1);
        //    curl_setopt($curl_request, CURLOPT_FOLLOWLOCATION, 0);
        //    $jsonEncodedData = json_encode($parameters);
        //    $post = array(
        //        "method" => $method,
        //        "input_type" => "JSON",
        //        "response_type" => "JSON",
        //        "rest_data" => $jsonEncodedData
        //    );
        //    curl_setopt($curl_request, CURLOPT_POSTFIELDS, $post);
        //    $result = curl_exec($curl_request);
        //    curl_close($curl_request);
        //    $result = explode("\r\n\r\n", $result, 2);
        //    $response = json_decode($result[1]);
        //    ob_end_flush();
        //    return $response;
        //}

        //Method example: login
        public void login()
        {
            JObject user_auth = new JObject();
            user_auth.Add("user_name", username);
            user_auth.Add("password", password);

            JObject parameter = new JObject();
            parameter.Add("user_auth", user_auth);
            parameter.Add("application_name", "RESTTEST");
            parameter.Add("name_value_list", null);
            JObject result = Call("login", parameter);
            
            //If we want more login result information:
            session = (string)result.GetValue("id");
            if (session != null) {
                Console.WriteLine("Suite session: " + session);
            }
            else
            {
                Console.WriteLine("Login failed: " + result.ToString());
            }
        }
        //If we use PHP cURL, "Call" should look like:

        //$login_parameters = array(
        //    "user_auth" => array(
        //        "user_name" => $username,
        //        "password" => md5($password),
        //    ),
        //    "application_name" => "My Application",
        //    "name_value_list" => array(
        //        array(
        //            'name' => 'language',
        //            'value' => 'en_us',
        //        ),
        //        array(
        //            'name' => 'notifyonsave',
        //            'value' => true
        //        ),
        //    ),
        //);


        //Method example: get_entry_list
        public JObject getEntryList(string module, string id, string name, string title)
        {
            JObject parameter = new JObject();
            parameter.Add("module_name", module);
            parameter.Add("query", "");
            parameter.Add("order_by", "");
            parameter.Add("offset", "0");

            JArray fields = new JArray(id, name, title);
            parameter.Add("select_fields", fields);

            parameter.Add("link_name_to_fields_array", new JArray());
            parameter.Add("max_results", "2");
            parameter.Add("deleted", "0");
            parameter.Add("Favorites", false);

            JObject result = Call("get_entry_list", parameter);
            return result;
        }
        //If we use PHP cURL, "Call" should look like:

        //$get_entry_list_parameters = array(
        //     'session' => $session_id,
        //     'module_name' => 'Leads',
        //     'query' => "",
        //     'order_by' => "",
        //     'offset' => '0',
        //     'select_fields' => array(
        //          'id',
        //          'name',
        //          'title',
        //     ),
        //     'link_name_to_fields_array' => array(
        //     ),
        //     'max_results' => '2',
        //     'deleted' => '0',
        //     'Favorites' => false,
        //);
        //$get_entry_list_result = call('get_entry_list', $get_entry_list_parameters, $url);
    }
}
