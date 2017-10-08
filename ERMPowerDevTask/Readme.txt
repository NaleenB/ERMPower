Assumptions
1. 20% above median means larger than value {median}*120/100 and not the 70th percentile
2. 20% below median means smaller than value {median}*80/100 and not the 30th percentile
3. CSV-s contain many different datat types and data values.
   But the median will be calculated for only one data type. 
   (calculating one median for different data types spoils the precision)
4. If different files contain different spellings or definitions for data types, they are known before hand and entered in to the program.
5. Only CSV files starting with "LP" and "TO" will be processed.

How To Run On Visual Studio
1. In App.config change "CSVLocation"
2. Change processor property enableAdditionalConsoleLogs to true to see additional logs on the console
2. Select ERMPowerDevTask as startup project and click start

How To Run Compiled
1. In ERMPowerDevTask.exe.config change "CSVLocation"
2. Run ERMPowerDevTask.exe