create table Log
(
    LogId INTEGER not null
        constraint PK_Log
            primary key autoincrement,
    Logged TEXT not null,
    LevelId INTEGER not null,
    Level TEXT,
    Message TEXT,
    Logger TEXT,
    Properties TEXT,
    Callsite TEXT,
    FileName TEXT,
    LineNumber INTEGER not null,
    Stacktrace TEXT,
    MachineName TEXT,
    Identity TEXT,
    Exception TEXT
);

create table Resource
(
    ResourceId INTEGER not null
        constraint PK_Resource
            primary key,
    ResourceName TEXT not null,
    ResourceType TEXT not null,
    NodeName TEXT not null,
    GalaxyName TEXT not null,
    FileName TEXT not null,
    AddedOn TEXT not null
);