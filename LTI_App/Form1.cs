//Exemplo comunicação API Rest do OpenStack em C#
//--------------------------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public String authToken = ""; // VARIAVEL PARA O TOKEN
        public String projId = "f383bbfa-0471-4bfe-92da-0ff08401f8ef"; // VARIAVEL PARA O ID DO PROJETO

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // EXEMPLO DE UM PEDIDO POST...
            //
            var myWebClient = new WebClient();
            myWebClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            // ...UTILIZANDO FORM-DATA
            //
            //var values = new NameValueCollection();
            //values["thing1"] = "hello";
            //values["thing2"] = "world";
            //var responseString = client.UploadValues("http://www.ltirocks.eu", values);


            // ...UTILIZANDO JSON
            //
            String jsonToSend = "{\"auth\": {\"identity\": {\"methods\": [\"password\"],\"password\": {\"user\": {\"id\": \"1a06c489fc0d41328e743f727223eddf\",\"password\": \"devstack\"}}},\"scope\": {\"project\": {\"id\": \"20283c29834d4245becc44bb34666069\"}}}}";
            var responseString = myWebClient.UploadString("http://127.0.0.1:8080/identity/v3/auth/tokens/", jsonToSend);
            WebHeaderCollection myWebHeaderCollection = myWebClient.ResponseHeaders;
            for (int i = 0; i < myWebHeaderCollection.Count; i++)
            {
                if (myWebHeaderCollection.GetKey(i) == "X-Subject-Token")
                {
                    authToken = myWebHeaderCollection.Get(i);
                }
            }

            MessageBox.Show(authToken, "Token de autenticação do OpenStack"); // MOSTRAR O TOKEN


            // EXEMPLO DE UM PEDIDO GET UTILIZANDO UM TOKEN DE AUTENTICAÇÃO
            //
            myWebClient = new WebClient();
            myWebClient.Headers.Add("x-auth-token", authToken);
            //String url = "http://127.0.0.1:8080/compute/v2.1/servers/" + projId;
            String url = "http://127.0.0.1:8080/compute/v2.1/servers";
            responseString = myWebClient.DownloadString(url);

            MessageBox.Show(responseString, "Resposta enviada do Openstack em Json"); // MOSTRAR O JSON RECEBIDO


            // CONVERTER DE JSON PARA XML (instalar a json.net via nugget)
            //
            XmlDocument xdoc = JsonConvert.DeserializeXmlNode(responseString);

            MessageBox.Show(xdoc.OuterXml, "XML criado a partir do Json recebido do OpenStack"); // MOSTRAR O XML CONVERTIDO


            // CONVERTER DE XML PARA JSON 
            //
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xdoc.OuterXml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);

            MessageBox.Show(jsonText, "Json criado a partir do XML convertido"); // MOSTRAR O JSON CONVERTIDO

        }
    }
}
