using FTPApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FtpApp.Models.Utilities
{
    public class FTP
    {
        public static List<string> GetDirectory(string url, string username = Constants.FTP.UserName, string password = Constants.FTP.Password)
        {
            List<string> output = new List<string>();

            //Build the WebRequest
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);

            request.Credentials = new NetworkCredential(username, password);

            request.Method = WebRequestMethods.Ftp.ListDirectory;
            //request.EnableSsl = false;
            request.KeepAlive = false;

            //Connect to the FTP server and prepare a Response
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                //Get a reference to the Response stream
                using (Stream responseStream = response.GetResponseStream())
                {
                    //Read the Response stream
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        //Retrieve the entire contents of the Response
                        string responseString = reader.ReadToEnd();

                        //Split the response by Carriage Return and Line Feed character to return a list of directories
                        output = responseString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

                        //Close the StreamReader
                        reader.Close();
                    }

                    //Close the Stream
                    responseStream.Close();
                }

                //Close the FtpWebResponse
                response.Close();
                Console.WriteLine($"Directory List Complete with status code: {response.StatusDescription}");
            }

            return (output);
        }

        /// <summary>
        /// Downloads a file from an FTP site
        /// </summary>
        /// <param name="sourceFileUrl">Remote file Url</param>
        /// <param name="destinationFilePath">Destination local file path</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Result of file download</returns>
        public static string DownloadFile(string sourceFileUrl, string destinationFilePath, string username = Constants.FTP.UserName, string password = Constants.FTP.Password)
        {
            string output;

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sourceFileUrl);

            //Specify the method of transaction
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(username, password);

            //Indicate Binary so that any file type can be downloaded
            request.UseBinary = true;
            request.KeepAlive = false;

            try
            {
                //Create an instance of a Response object
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    //Request a Response from the server
                    using (Stream stream = response.GetResponseStream())
                    {
                        //Build a variable to hold the data using a size of 1Mb or 1024 bytes
                        byte[] buffer = new byte[1024]; //1 Mb chucks

                        //Establish a file stream to collect data from the response
                        using (FileStream fs = new FileStream(destinationFilePath, FileMode.Create))
                        {
                            //Read data from the stream at the rate of the size of the buffer
                            int bytesRead = stream.Read(buffer, 0, buffer.Length);

                            //Loop until the stream data is complete
                            while (bytesRead > 0)
                            {
                                //Write the data to the file
                                fs.Write(buffer, 0, bytesRead);

                                //Read data from the stream at the rate of the size of the buffer
                                bytesRead = stream.Read(buffer, 0, buffer.Length);
                            }

                            //Close the StreamReader
                            fs.Close();
                        }

                        //Close the Stream
                        stream.Close();
                    }

                    //Close the FtpWebResponse
                    response.Close();

                    //Output the results to the return string
                    output = $"Download Complete, status {response.StatusDescription}";
                }

            }
            catch (WebException e)
            {
                FtpWebResponse response = (FtpWebResponse)e.Response;
                //Something went wrong with the Web Request
                output = e.Message + $"\n Exited with status code {response.StatusCode}";
            }
            catch (Exception e)
            {
                //Something went wrong
                output = e.Message;
            }

            //Return the output of the Responce
            return (output);
        }

        internal static bool FileExists(object p)
        {
            throw new NotImplementedException();
        }

        internal static object DownloadFileBytes(object p)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Converts the contents of the file to a byte array
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetStreamBytes(string filePath)
        {
            using (StreamReader sourceStream = new StreamReader(filePath))
            {
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                return fileContents;
            }
        }

        /// <summary>
        /// Convert a Stream to a byte array
        /// </summary>
        /// <param name="stream">A Stream Object</param>
        /// <returns>Array of bytes from the Stream</returns>
        public static byte[] ToByteArray(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] chunk = new byte[1024];
                int bytesRead;
                while ((bytesRead = stream.Read(chunk, 0, chunk.Length)) > 0)
                {
                    ms.Write(chunk, 0, bytesRead);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Tests to determine whether a file exists on an FTP site
        /// </summary>
        /// <param name="remoteFileUrl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool FileExists(string remoteFileUrl, string username = Constants.FTP.UserName, string password = Constants.FTP.Password)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFileUrl);

            //Specify the method of transaction
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.KeepAlive = false;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(username, password);

            try
            {
                //Create an instance of a Response object
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                //Close the FtpWebResponse
                response.Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    //Does not exist
                    return false;
                }
            }
            return true;
        }

        public static byte[] DownloadFileBytes(string sourceFileUrl, string username = Constants.FTP.UserName, string password = Constants.FTP.Password)
        {
            byte[] output;

            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sourceFileUrl);

                //Specify the method of transaction
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(username, password);

                //Create an instance of a Response object
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    //Request a Response from the server
                    output = ToByteArray(response.GetResponseStream());

                    response.Close();

                    //Return the output of the Response
                    return output;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new byte[0];
        }
            //endregion


    }
}
