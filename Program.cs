using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace main;

class Vehicle
{

    public string Type
    { get; set; }

    public string Plat
    { get; set; }

    public string Color
    { get; set; }

    public int Slot
    { get; set; }

    public Vehicle(string modelPlat, string modelColor, string modelType, int modelSlot)
    {
        Type = modelType;
        Plat = modelPlat;
        Color = modelColor;
        Slot = modelSlot;
    }
}
class Program
{
    public static int parkingLot = 0;
    public static int occupiedLot = 0;
    public static bool isActive = true;
    public static List<Vehicle> vehicleList = new List<Vehicle>();

    public static void Main()
    {
        do
        {
            home();
            var res = checkInput();
            menu(res.Item1, res.Item2);
        } while (isActive);
    }

    static void home()
    {

    }


    static (string, string) checkInput()
    {
        Console.Write("write your input: ");
        string? input = Console.ReadLine();
        string[] pattern = {
            "^create_parking_lot",
            "^park",
            "^leave",
            "^status",
            "^type_of_vehicles",
            "^registration_numbers_for_vehicles_with_color",
            "^registration_numbers_for_vehicles_with_",
            "^slot_number_for_registration_number",
            "^slot_number_for_vehicles_with_color"
        };

        string res = "";
        foreach (string item in pattern)
        {
            var m = Regex.Match(input, item);
            if (m.Value == item.Substring(1))
            {
                res = m.Value;
                break;
            }
        }
        if (input == "exit")
        {
            res = "exit";
        }
        else if (input == "list")
        {
            res = "list";
        }
        return (input, res);
    }
    static void menu(string input, string code)
    {
        if (input == "")
        {
            Console.WriteLine("input is empty!");
            return;
        }
        if (code != "create_parking_lot")
        {
            if (parkingLot == 0)
            {
                Console.WriteLine("Parking lot must be inputted!");
                return;
            }
        }

        switch (code)
        {
            case "create_parking_lot":
                createParkLot(input);
                break;
            case "park":
                if (vehicleList.Count() >= parkingLot && vehicleList[parkingLot - 1] != null)
                {
                    Console.WriteLine("Sorry, parking lot is full");
                    return;
                }
                parkVehicle(input);
                break;
            case "leave":
                leaveVehicle(input);
                break;

            case "type_of_vehicles":
                countVehicleByType(input);
                break;

            case "registration_numbers_for_vehicles_with_color":
                getVehicleRegByColor(input);
                break;
            case "registration_numbers_for_vehicles_with_":
                getOddEvenVehicleReg(input);
                break;
            case "slot_number_for_registration_number":
                getSlotByRegNumber(input);
                break;
            case "slot_number_for_vehicles_with_color":
                getSlotByColor(input);
                break;
            case "status":
                status();
                break;

            case "exit":
                isActive = false;
                break;
            case "":
                Console.WriteLine("input is empty or invalid!");
                break;
            case "list":
                List();
                break;
        }
    }

    static void createParkLot(string input)
    {
        string checkPattern = @"^create_parking_lot\s\d{1,}$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use create_parking_lot {number-of-lot}");
            return;
        }
        else
        {
            var captureLot = Regex.Match(input, @"\d{1,}");
            parkingLot = Convert.ToInt32(captureLot.Value);
            if (parkingLot > 50)
            {
                Console.WriteLine("cannot create more than 50 lots!");
            }
            Console.WriteLine($"Created a parking lot with {parkingLot} slot");
        }
    }

    static void parkVehicle(string input)
    {
        string checkPattern = @"^park\s\w{1}\-\d{4}\-\w{3}\s\w{4,}\s\w{5}$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use park [uhh]-[num]-[word] [color] [type]");
            return;
        }
        else
        {
            string platPattern = @"\b\w{1}\-\d{4}\-\w{3}\b";
            string colorPattern = @"(?<=\b\w{1}\-\d{4}\-\w{3}\s\b)\b\w{4,}\b";
            string typePattern = @"(?<=\b\w{4,}\s\b)\b\w{5}$\b";

            var getPlat = Regex.Match(input, platPattern);
            var getColor = Regex.Match(input, colorPattern);
            var getType = Regex.Match(input, typePattern);

            int slotNum = occupiedLot + 1;

            // skenario 1,3 lot 3
            // skenario 4 lot 5
            // skenario 2,3 lot 3

            // int slotNum = 0; //2
            if (vehicleList.Count == 0)
            {
                slotNum = 1;
            }
            else
            {
                bool isAdded = false;
                for (int i = 0; i < vehicleList.Count; i++)//1
                {
                    if (vehicleList[i] == null)
                    {
                        slotNum = i + 1;
                        // Console.WriteLine($"this get calld");

                        Vehicle missingVehicle = new Vehicle(getPlat.Value.ToUpper(), getColor.Value.ToLower(), getType.Value.ToLower(), slotNum);
                        vehicleList[i] = missingVehicle;
                        isAdded = true;
                        Console.WriteLine($"Allocated slot number: {slotNum}");
                        break;
                    }
                }
                if (isAdded)
                {
                    return;
                }
            }

            Vehicle vehicle = new Vehicle(getPlat.Value.ToUpper(), getColor.Value.ToLower(), getType.Value.ToLower(), slotNum);
            vehicleList.Add(vehicle);
            // set occupiedLot
            occupiedLot = vehicleList.Count;

            Console.WriteLine($"Allocated slot number: {slotNum}");
        }
    }

    static void leaveVehicle(string input)
    {
        string checkPattern = @"^leave\s\d{1,}$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use leave [slot]");
            return;
        }
        else
        {
            string slotPattern = @"\d{1,}$";
            var getSlot = Regex.Match(input, slotPattern);

            // Vehicle leavingVehicle = null;
            // foreach (Vehicle v in vehicleList)
            // {
            //     if (v.Slot == Convert.ToInt32(getSlot.Value))
            //     {
            //         leavingVehicle = v;

            //         break;
            //     }
            // }

            for (int i = 0; i < vehicleList.Count; i++)
            {
                if (vehicleList[i] == null)
                {
                    continue;
                }
                if (vehicleList[i].Slot == Convert.ToInt32(getSlot.Value))
                {
                    vehicleList[i] = null;
                }
            }
            // vehicleList.Remove(leavingVehicle);
            Console.WriteLine($"Slot number {getSlot.Value} is free");
        }
    }

    static void countVehicleByType(string input)
    {
        string checkPattern = @"^type_of_vehicles\s\w{5}$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use type_of_vehicles [type]");
            return;
        }
        else
        {
            string typePattern = @"(?:mobil|motor)";
            var getType = Regex.Match(input, typePattern);
            Console.WriteLine($"{getType.Value}");

            List<Vehicle>? typeTotal = new List<Vehicle>();
            foreach (var item in vehicleList)
            {
                if (item.Type == getType.Value.ToLower())
                {
                    typeTotal.Add(item);
                }
            }
            int total = typeTotal.Count();
            Console.WriteLine($"{total}");
        }
    }

    static void getVehicleRegByColor(string input)
    {
        string checkPattern = @"^registration_numbers_for_vehicles_with_color\s\w{4,}$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use registration_numbers_for_vehicles_with_color [color]");
            return;
        }
        else
        {
            string colorPattern = @"\w{5}$";
            var getPlat = Regex.Match(input, colorPattern);
            Console.WriteLine($"{getPlat.Value}");

            List<string>? regNum = new List<string>();
            foreach (var item in vehicleList)
            {
                if (item.Color == getPlat.Value.ToLower())
                {
                    regNum.Add(item.Plat);
                    Console.Write($"{item.Plat}, ");
                }
            }
            Console.WriteLine($" ");
        }
    }

    static void getOddEvenVehicleReg(string input)
    {
        string checkPattern = @"^registration_numbers_for_vehicles_with_\w{3,}_plate$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use registration_numbers_for_vehicles_with_[odd/even]_plate");
            return;
        }
        else
        {
            string evenoddPattern = @"(?:even|odd)";
            string platePattern = @"\d{4}";
            string getevenodd = Regex.Match(input, evenoddPattern).Value;

            List<string>? regNum = new List<string>();
            foreach (var item in vehicleList)
            {
                var plat = Regex.Match(item.Plat, platePattern);
                int convPlat = Convert.ToInt32(plat.Value) % 2;

                if (convPlat == 0 && getevenodd == "even")
                {
                    regNum.Add(item.Plat);
                    Console.Write($"{item.Plat}, ");
                }
                else if (convPlat == 1 && getevenodd == "odd")
                {
                    regNum.Add(item.Plat);
                    Console.Write($"{item.Plat}, ");
                }
            }
            Console.WriteLine($" ");
        }
    }

    static void getSlotByRegNumber(string input)
    {
        string checkPattern = @"^slot_number_for_registration_number\s\b\w{1}\-\d{4}\-\w{3}\b$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use slot_number_for_registration_number [plate]");
            return;
        }
        else
        {
            string platePattern = @"\b\w{1}\-\d{4}\-\w{3}\b$";
            string getPlate = Regex.Match(input, platePattern).Value;
            Console.WriteLine($"{getPlate}");

            bool isExist = false;
            foreach (var item in vehicleList)
            {
                if (item.Plat == getPlate)
                {
                    Console.WriteLine($"{item.Slot}");
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                Console.WriteLine($"Not Found");
            }
        }
    }

    static void getSlotByColor(string input)
    {
        string checkPattern = @"^slot_number_for_vehicles_with_color\s\w{4,}$";
        if (!Regex.IsMatch(input, checkPattern))
        {
            Console.WriteLine("command is wrong! use slot_number_for_vehicles_with_color [color]");
            return;
        }
        else
        {
            string colorPattern = @"\w{5}$";
            string getColor = Regex.Match(input, colorPattern).Value;
            Console.WriteLine($"{getColor}");

            foreach (var item in vehicleList)
            {
                if (item.Color == getColor)
                {
                    Console.Write($"{item.Slot}, ");
                }
            }
            Console.WriteLine($" ");
        }
    }

    static void status()
    {
        Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-5}", "Slot", "Plate", "Type", "Color");
        Console.WriteLine(new string('-', 55)); // Table header separator        
        foreach (var item in vehicleList)
        {
            if (item == null)
            {
                Console.WriteLine($"empty");
            }
            else
            {
                Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-5}", item.Slot, item.Plat, item.Type, item.Color);
            }
        }
    }

    // for test only method
    static void List()
    {
        foreach (var item in vehicleList)
        {
            if (item == null)
            {
                Console.WriteLine($"empty");
            }
            else
            {
                Console.WriteLine($"{item.Slot}");
            }
        }
    }
}