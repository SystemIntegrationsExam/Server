using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using Server.ApiHelper;
using Server.Microservices;
using Server.Model;


namespace Server
{
    class Receive
    {
        //Aggregation fields 
        private static string message1;  // Type and Date
        private static string message2;  // Color
        private static string message3;  // Driver name and license  
        //private static string logPath = @"C:/Users/Bruger/source/repos/Mini-Project-3-microservices/Log.txt";    // Christoffer
        //private static string logPath = "C:/Users/Jonas/source/repos/Mini-Project-3-microservices/Log.txt";      // Jonas mac 
        private static string logPath = "C:/Users/Bruger/Documents/GitHub/Server/Log.txt";   // Jonas


        static async Task Main(string[] args)
        {
            //             [MicroService used]                       //   
            Apihelper.InitializeClient();
            Microservices.CarService.Processor processor = new Microservices.CarService.Processor();
            await processor.getBookings();


            //EIP "Channel" created on localhost with help of MqRabbit ref.. 
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Declaring of the que to send messages to. 
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                //Variable used within the Received protocol. 
                int caseSwitch = 1;
                int caseSwitchChoice = 1;
                List<Car> availableCars = new List<Car>();
                List<string> colorsFound = new List<string>();
                List<Car> availableCarsByColor = new List<Car>();
                List<Car> chooseCarList = new List<Car>();
                Car selectedCar = null;
                bool found = false;


                Microservices.ReviewService.Processor reviewProcessor = new Microservices.ReviewService.Processor();

                //Message recevied
                consumer.Received += async (model, ea) =>
                {
                    string response = null;
                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    var message = Encoding.UTF8.GetString(body).ToString();
                    //Console.WriteLine(" [.] Message send from client", message);
                    string[] opdelt = message.Split(' ');
                    var identifyer = opdelt[0];

                    File.AppendAllText(logPath, TimeStampForLog(" ", message));
                    //write message down into a TXT log file (not made yet) 

                    //Choice 1 - Review 
                    if (identifyer == "R")
                    {
                        switch (caseSwitchChoice)
                        {
                            case 1:
                                File.AppendAllText(logPath, Setuplog("----------------------------Flow start----------------------------"));
                                Review review = new Review();
                                string section1 = "Feedback section";
                                Console.WriteLine(section1);
                                File.AppendAllText(logPath, TimeStampForLog(section1, message));

                                var responseBytes = Encoding.UTF8.GetBytes("R");
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                             basicProperties: replyProps, body: responseBytes);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);

                                caseSwitchChoice += 1;
                                break;
                            case 2:
                                string section2 = "Choice case 1 - rating";
                                Console.WriteLine(section2);
                                File.AppendAllText(logPath, TimeStampForLog(section2, message));
                                string realMessageRating = message.Remove(0, 1);
                                reviewProcessor.Review.Rating = Convert.ToInt32(realMessageRating);

                                response = "rating ok";
                                File.AppendAllText(logPath, TimeStampForLog(response, message));

                                var responseRating = Encoding.UTF8.GetBytes(response);

                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                     basicProperties: replyProps, body: responseRating);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);

                                caseSwitchChoice += 1;
                                break;
                            case 3:
                                string section3 = "Choice case 2 - Location";
                                Console.WriteLine(section3);
                                File.AppendAllText(logPath, TimeStampForLog(section3, message));
                                string realMessageLocation = message.Remove(0, 1);
                                reviewProcessor.Review.Location = realMessageLocation;

                                response = "Location registered";
                                File.AppendAllText(logPath, TimeStampForLog(response, message));

                                var responseLocation = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                     basicProperties: replyProps, body: responseLocation);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);

                                caseSwitchChoice += 1;
                                break;
                            case 4:
                                string section4 = "Choice case 3 - description";
                                Console.WriteLine(section4);
                                File.AppendAllText(logPath, TimeStampForLog(section4, message));
                                string realMessagedescription = message.Remove(0, 1);
                                reviewProcessor.Review.Description = realMessagedescription;

                                response = "description registered ok";
                                File.AppendAllText(logPath, TimeStampForLog(response, message));

                                var respondescription = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                     basicProperties: replyProps, body: respondescription);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);
                                caseSwitchChoice += 1;
                                break;
                            case 5:
                                string section5 = "Choice case 4 - gender";
                                Console.WriteLine(section5);
                                File.AppendAllText(logPath, TimeStampForLog(section5, message));
                                string realMessageGender = message.Remove(0, 1);
                                reviewProcessor.Review.Gender = realMessageGender;

                                response = "gender registered ok";
                                File.AppendAllText(logPath, TimeStampForLog(response, message));

                                var responseMale = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                     basicProperties: replyProps, body: responseMale);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);
                                caseSwitchChoice += 1;
                                break;
                            case 6:
                                string section6 = "Choice case 5 - age";
                                Console.WriteLine(section6);
                                File.AppendAllText(logPath, TimeStampForLog(section6, message));
                                string realMessageAge = message.Remove(0, 1);

                                reviewProcessor.Review.Age = Convert.ToInt32(realMessageAge);

                                response = "Age registered ok";
                                File.AppendAllText(logPath, TimeStampForLog(response, message));

                                var responseAge = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                     basicProperties: replyProps, body: responseAge);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);

                                await reviewProcessor.PostReview();

                                //reset the server variable controlling the flow, so it can start over. 
                                caseSwitch = 1;
                                caseSwitchChoice = 1;
                                selectedCar = null;
                                found = false;
                                identifyer = " ";
                                File.AppendAllText(logPath, Setuplog("----------------------------Flow end----------------------------"));
                                break;
                            default:
                                string defaultCaseString = "Default case";
                                Console.WriteLine(defaultCaseString);
                                File.AppendAllText(logPath, TimeStampForLog(defaultCaseString, message));
                                break;
                        }
                    }
                    else
                    {
                        //Choice 2 - create booking
                        switch (caseSwitch)
                        {
                            case 1:
                                File.AppendAllText(logPath, Setuplog("----------------------------Flow start----------------------------"));
                                string section2_1 = "Create Section";
                                Console.WriteLine(section2_1);
                                File.AppendAllText(logPath, TimeStampForLog(section2_1, message));
                                response = "2";
                                var responseBytesChoice = Encoding.UTF8.GetBytes(response);

                                //Send a response back ---------------
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                  basicProperties: replyProps, body: responseBytesChoice);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);

                                caseSwitch += 1;
                                break;
                            //Check avaliablityart
                            case 2:
                                File.AppendAllText(logPath, TimeStampForLog("Feedback section = 2", message));
                                Console.WriteLine("Case 1 - Check avaliablity");
                                message1 = message;
                                string statusTest1 = "Case 1 - Check avaliablity";
                                File.AppendAllText(logPath, TimeStampForLog(statusTest1, message));

                                //EIP - Splitter  ---------------------------------------
                                //Splitting the data so we can use it to search the list in DataStorage for availablity
                                String[] messageArray = message.Split(' ');
                                string carType = messageArray[0];
                                string date = messageArray[1];
                                //EIP - Splitter ---------------------------------------


                                foreach (var car in processor.CarList)
                                {
                                    if (car.Type == carType && car.Date == date)
                                    {
                                        found = true;
                                        availableCars.Add(car);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                if (found == true)
                                {
                                    response = "bil blev fundet";
                                    Console.WriteLine("bil blev fundet");
                                    File.AppendAllText(logPath, TimeStampForLog(statusTest1, response));

                                    //EIP - Request & Reply ------------------------------------
                                    //New message created -----------------
                                    var responseBytes = Encoding.UTF8.GetBytes(response);
                                    //New message created -----------------

                                    //Send a response back ---------------
                                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                      basicProperties: replyProps, body: responseBytes);
                                    channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                      multiple: false);
                                    //Send a response back ---------------
                                    //EIP - Request & Reply ------------------------------------

                                    caseSwitch += 1;
                                }
                                else
                                {
                                    response = "bil blev ikke fundet";
                                    Console.WriteLine("bil blev ikke fundet");
                                    File.AppendAllText(logPath, TimeStampForLog(statusTest1, response));

                                    var responseBytes = Encoding.UTF8.GetBytes(response);
                                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                      basicProperties: replyProps, body: responseBytes);
                                    channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                      multiple: false);
                                }
                                break;
                            //Choose color 
                            case 3:
                                string section2_3 = "case 2 - choose color";
                                Console.WriteLine(section2_3);
                                message2 = message;
                                File.AppendAllText(logPath, TimeStampForLog(section2_3, message));

                                foreach (var car in availableCars)
                                {
                                    if (colorsFound.Contains(car.Color))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        colorsFound.Add(car.Color);
                                    }
                                }

                                foreach (var color in colorsFound)
                                {
                                    response = response + " " + color;
                                }
                                if (response == string.Empty)
                                {
                                    response = "Color not found, please try with another one or check spelling";
                                    File.AppendAllText(logPath, TimeStampForLog(section2_3, response));
                                }
                                else
                                {
                                    File.AppendAllText(logPath, TimeStampForLog(section2_3, response));
                                    var responseBytesCase2 = Encoding.UTF8.GetBytes(response);
                                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                      basicProperties: replyProps, body: responseBytesCase2);
                                    channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                      multiple: false);
                                    caseSwitch += 1;
                                }
                                break;
                            case 4:
                                string section2_4 = "case 3 - choose car";
                                Console.WriteLine(section2_4);
                                File.AppendAllText(logPath, TimeStampForLog(section2_4, message));
                                if (message == string.Empty)
                                {
                                    response = "Please type a color to preceed";
                                    File.AppendAllText(logPath, TimeStampForLog(section2_4, response));
                                }
                                else if (colorsFound.Contains(message) == false)
                                {
                                    response = "Spelling ERORR, please try agian";
                                    File.AppendAllText(logPath, TimeStampForLog(section2_4, response));
                                }
                                else
                                {
                                    foreach (var car in availableCars)
                                    {
                                        if (car.Color == message)
                                        {
                                            availableCarsByColor.Add(car);
                                        }
                                    }
                                    foreach (var car in availableCarsByColor)
                                    {
                                        response = response + car.ToString() + "-";

                                        chooseCarList.Add(car);
                                    }

                                    File.AppendAllText(logPath, TimeStampForLog(section2_4, response));
                                    caseSwitch += 1;
                                }

                                var responseBytesCase3 = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                  basicProperties: replyProps, body: responseBytesCase3);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);

                                break;
                            case 5:
                                string section2_5 = "case 4 - selecet car";
                                Console.WriteLine(section2_5);
                                File.AppendAllText(logPath, TimeStampForLog(section2_5, message));
                                int index = Convert.ToInt32(message) - 1;
                                if (index < 0 || availableCarsByColor.Count < index + 1)
                                {
                                    response = "The number is not valid, try with a differnt one";
                                    File.AppendAllText(logPath, TimeStampForLog(section2_5, response));
                                }
                                else
                                {
                                    selectedCar = chooseCarList[index];
                                    response = "selected CAR: " + selectedCar.ToString();
                                    File.AppendAllText(logPath, TimeStampForLog(section2_5, response));
                                    caseSwitch += 1;
                                }

                                File.AppendAllText(logPath, TimeStampForLog(section2_5, message));
                                var responseBytesCase4 = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                  basicProperties: replyProps, body: responseBytesCase4);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);
                                break;
                            case 6:
                                string section2_6 = "case 5 - create booking";
                                Console.WriteLine(section2_6);
                                File.AppendAllText(logPath, TimeStampForLog(section2_6, message));

                                message3 = message;
                                string[] nameAndLicense = message.Split(' ');
                                string name = nameAndLicense[0];
                                string license = nameAndLicense[1];
                                Driver driver = new Driver(name, license);
                                Booking booking = new Booking(driver, selectedCar, DateTime.Now);


                                //EIP - Aggregator  ------------------------------------------
                                if (message1 != string.Empty && message2 != string.Empty && message3 != string.Empty)
                                {
                                    response = message1 + message2 + message3;
                                    File.AppendAllText(logPath, TimeStampForLog(section2_6, response));
                                }
                                //EIP - Aggregator  ------------------------------------------

                                //Saves informations in a TXT file (illustrate database) 
                                File.AppendAllText(@"C:/Users/Bruger/Documents/GitHub/Server/CompletedRentals.txt", CompletePost(response));  //Jonas
                                //File.AppendAllText(@"C:/Users/Jonas/source/repos/Mini-Project-3-microservices/CompletedRentals.txt", CompletePost(response));  //Jonas mac
                                //File.AppendAllText(@"C:/Users/Bruger/source/repos/Mini-Project-3-microservices/CompletedRentals.txt", CompletePost(response)); //Christoffer
                                Console.WriteLine("Booking has been created");
                                File.AppendAllText(logPath, TimeStampForLog("section2_6", "Booking has been created"));

                                var responseBytesCase5 = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                  basicProperties: replyProps, body: responseBytesCase5);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);

                                File.AppendAllText(logPath, Setuplog("----------------------------Flow end----------------------------"));
                                //reset the server variable controlling the flow, so it can start over. 
                                caseSwitch = 1;
                                caseSwitchChoice = 1;
                                selectedCar = null;
                                found = false;
                                identifyer = " ";

                                break;
                            default:
                                string defaultCaseString = "Default case";
                                Console.WriteLine(defaultCaseString);
                                File.AppendAllText(logPath, TimeStampForLog(defaultCaseString, message));
                                break;
                        }
                    }
                };
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static string TimeStampForLog(string casestatus, string message)
        {
            string response = DateTime.Now + "    -    " + casestatus + "   -    with message: " + message + Environment.NewLine; ;
            return response;
        }
        private static string Setuplog(string message)
        {
            string response = message + Environment.NewLine; ;
            return response;
        }
        private static string CompletePost(string information)
        {
            string response = "Created: " + DateTime.Now + " --  Rented by: " + information + Environment.NewLine; ;
            return response;
        }
    }
}
