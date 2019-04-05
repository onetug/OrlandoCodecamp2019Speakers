# From Batches to Streams with Apache Kafka
## Orlando Code Camp 2019 Presentation

Presentation
[Presentation-Batches_to_Streams_with_Apache_Kafka.pdf](Presentation-Batches_to_Streams_with_Apache_Kafka.pdf)

I plan to do a YouTube video with what was presented at Orlando Code Camp, and when I get that done I'll drop the link here.

### Steps
*NOTE*: I apologize that the steps below seem a little disjointed, but the reality is that in a REAL production application that all these various pieces, this is the nature of the beast.  Honestly, stepping through all of these will give an appreciation for the types of interactions that exist in a real environment rather than just being able to push a button and have everything magically happen.

1. Download the SQL Server JDBC Driver
https://www.microsoft.com/en-us/download/details.aspx?id=57782

2. Unzip the mssql-jdbc-7.2.1.jre8.jar to ./kafka-connect/jars folder
NOTE: When unzipping the jdbc zip, my jar was located in sqljdbc_7.2.1.0_enu.tar\sqljdbc_7.2.1.0_enu\sqljdbc_7.2\enu

3. Start the stack - look in the [docker-compose.yml](docker-compose.yml) file to see a list of the services that will get started
`docker-compose up -d`

2. See if SQL Server is alive
`docker-compose logs --tail 100 sql-server`

3. Run the Create DB Scripts - I used SSMS
[db-01.sql](db-01.sql)

3. Run the select scripts to look at the rewards / transaction tables - do this in another tab in SSMS
[db-02.sql](db-02.sql)

4. Start up Postman - Get it here <https://www.getpostman.com/downloads/>

5. Import Postman Scripts [OrlandoCC.postman_collection.json](OrlandoCC.postman_collection.json)

6. Run the User Rewards Source Connector in Postman

7. Launch Confluent Control Center at http://localhost:9021/

8. Look at the Topics tab

9. After we see some data in the rewards topic, run the User Rewards Transactions Source Connector in Postman

10. Streams Time - Open up [ksql-streams.txt](ksql-streams.txt) and run the blocks of statments one at a time

11. Run the Sink Connectors
    1. User Rewards Real Time DB 
    2. User Rewards Real Time Elastic

