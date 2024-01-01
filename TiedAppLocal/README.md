# TiedAppLocal API

This project is designed for companies which would like to use TiedApp and store data in their own server. 
Please follow the setup and customization steps by order to avoid bad configuration.

## CSS Style
You can configure your own css by replacing ours.
The css path is "TiedAppLocal\wwwroot\css\site.css".

## Requirement
- [Visual Studio](https://visualstudio.microsoft.com/fr/) - mandatory
- [.net core 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) - mandatory

## Warning
Please do not modify the code inside the provided API unless it is mention that it can be modified.
For example: any modification of [httppost] to [httpget] will make the use of the tiedapp solution impossible with your deployed API.

## Step 1. Set your preferred configuration
1. In 'Program.cs' you will find under 'Special Dir' three type of directory settings. Those directories can be modify.
2. To modify the directories, it recommended  to update the names inside the sub project 'GlobalShared' and the static class 'LibVariables'.
3. It is possible to create new controllers and or change the name of the action inside the controller.
4. For example: you can create a controller 'PostFilesController' and copy in this controller all related codes for 'SaveFile' action inside.
5. You can modify the name of controller or the name of the action. Do not under any circumstances modify the type of operation [Http...]

## Step 2. Deploy your API
Once all the setup of your API is done, you can deploy it on your own server.
Once your API is deployed, please make sure to setup read and write permissions for your apppool to the directories in which files will be uploaded if you are using IIS.

## Step 3. Setup TiedApp for all employees
Only the direction and the IT department can setup the API in TiedApp.
To setup your API for all employees, the following informations will be needed in TiedApp:
1. The url to access your API deployed,
2. The controller name and action to post a file,
3. The controller name and action to delete a file posted using a task management procedure which was rejected,
4. The controller name and action to retrieve a file posted.

-- Important note --
In TiedApp, once the API is activated, any process related to a file to post will forcibly use the deployed API's features.
In case you want to do maintenance on your server, you can 'deactivated' the use of your API in TiedApp.
When your API is activated, all files posted by any employee will automatically be posted to your server.
for more informations please follow the update details of TiedApp [@here](https://tiedapp.com/TiedAppUpdateDetail/1012)

## API's processes security
To ensure the security of your API, we created a token validation process.
TiedApp application will retrieve a validation token from our server, the validation token will be encrypted then insert in the object linked to the Http request. 
In your deployed API, the token will be sended for validation to our server. The process request by TiedApp will only be validated by our server. 
If not, the Http request will not be successful.

## Authors

- [@flaubertagu](https://github.com/flaubertagu)

## Support

For support, email support@tiedapp.com
