# Attendance Notification

Attendance Notification is a .NET application designed to automate the process of checking attendance from a Google Sheet and sending timely notifications to a LINE group, highlighting those who have not yet completed the required daily survey.

This tool is perfect for teams or departments that need to track daily participation in surveys or other required check-ins. It includes separate configurations for two different departments: DSI and System D\&D.

## Key Features

  * **Automated Attendance Checking**: Downloads the latest attendance data directly from a Google Sheet.
  * **LINE Group Notifications**: Sends customized notifications to a designated LINE group.
  * **Identifies Missing Entries**: Compares the list of attendees against a master list of employee IDs and identifies those who are missing.
  * **Customizable for Different Departments**: Includes separate configurations for "DSI" and "System D\&D" departments.
  * **Error Logging**: Automatically logs any errors to a text file for easy troubleshooting.
  * **Scheduled Execution**: Can be easily configured to run automatically at specific times using Windows Task Scheduler.

## How It Works

The application performs the following steps when executed:

1.  **Downloads Data**: It fetches the latest attendance data as a CSV file from a specified Google Sheet URL.
2.  **Reads Employee Lists**: It reads the master list of employee IDs from a local text file.
3.  **Compares and Filters**: The application compares the downloaded attendance data with the master list to find out who has not yet submitted their attendance for the current day.
4.  **Sends Notification**: It then sends a message to the configured LINE group, listing the employees who still need to complete the survey and provides a direct link to the form.
5.  **Handles Completion**: If everyone has completed the survey, it sends a confirmation message and closes the application.

## Technologies Used

  * **.NET 6**: The application is built using the modern and cross-platform .NET 6 framework.
  * **Windows Forms**: The user interface is a simple Windows Forms application.
  * **C\#**: The application is written in the C\# programming language.

## Setup and Installation

To get the Attendance Notification system up and running, follow these steps:

### 1\. Configure the Application

Before running the application, you need to configure the settings in the `App.config` file for each department (DSI and SystemD\&D).

#### For the DSI Department:

Open `DSI/AttendanceNotification/AttendanceNotification/App.config` and update the following values:

```xml
<appSettings>
    <add key="Token" value="YOUR_LINE_NOTIFY_TOKEN_FOR_DSI"/>
    <add key="ReadFilePath" value="C:\path\to\your\Attendance.csv"/>
    <add key="ReadFileID" value="C:\path\to\your\CheckID_DSI.txt"/>
    <add key="Logfile" value="C:\path\to\your\LogError.txt"/>
</appSettings>
```

#### For the SystemD\&D Department:

Open `SystemD&D/AttendanceNotification/AttendanceNotification/App.config` and update the following values:

```xml
<appSettings>
    <add key="Token" value="YOUR_LINE_NOTIFY_TOKEN_FOR_DD"/>
    <add key="ReadFilePath" value="C:\path\to\your\Attendance.csv"/>
    <add key="ReadFileID" value="C:\path\to\your\CheckID_DD.txt"/>
    <add key="Logfile" value="C:\path\to\your\LogError.txt"/>
</appSettings>
```

### 2\. Set Up the Executable

Place the compiled executable file (`AttendanceNotification.exe`) in a suitable location on your server or computer.

### 3\. Schedule the Task

To automate the process, you can use Windows Task Scheduler to run the application at your desired time each day.

1.  Open **Task Scheduler**.
2.  Create a new task.
3.  Set a **trigger** for the time you want the notification to be sent (e.g., daily at 4:00 PM).
4.  Set the **action** to start a program and browse to the location of your `AttendanceNotification.exe` file.
5.  Save the task.

## Project Structure

The project is organized into two main folders, one for each department, with identical codebases but different configurations.

```
AttendanceNotification-main/
├── DSI/
│   └── AttendanceNotification/
│       ├── AttendanceNotification/
│       │   ├── App.config
│       │   ├── Form1.cs
│       │   └── ...
│       └── ...
└── SystemD&D/
    └── AttendanceNotification/
        ├── AttendanceNotification/
        │   ├── App.config
        │   ├── Form1.cs
        │   └── ...
        └── ...
```
