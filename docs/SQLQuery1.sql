CREATE TABLE Calander (
	calanderKey INT NOT NULL,
	calanderName VARCHAR(50),
	PRIMARY KEY(calanderKey)
)
CREATE TABLE CalanderEvent (
	eventID INT NOT NULL,
	eventName VARCHAR(50),
	startTime DATE,
	endTime DATE,
	PRIMARY KEY(eventID),
	calanderKey int FOREIGN KEY REFERENCES Calander(calanderKey)
)
GO

ALTER TABLE AspNetUserRoles 
ADD calanderKey int;
GO

SELECT * FROM Calander
GO

DROP TABLE Calander
DROP TABLE CalanderEvent
GO