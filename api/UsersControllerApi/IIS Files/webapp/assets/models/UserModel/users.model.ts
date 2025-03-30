export class DatabaseUsersModel {
    id: number;
    userId: string;
    usersname: string;
    password: string;
    emailAddress: string;
    contactNumber: string;

    constructor() {

        this.id = 0;
        this.userId = "";
        this.usersname = "Johan Botha";
        this.password = "5404271";
        this.emailAddress = "johan.botha@payteq.com";
        this.contactNumber = "0635906143";

        // this.id = 0;
        // this.userId = "";
        // this.usersname = "";
        // this.password = "";
        // this.emailAddress = "";
        // this.contactNumber = "";
        
    }

    GenNewUserNumber() {
        let epoc = Date.now();
        this.userId = "USR" + epoc;
        this.emailAddress += epoc;
    }
}

export class UserLoginModel {
    userName: string;
    password: string;

    constructor() {
        this.userName = "";
        this.password = "";
        
    }
}

export class GetDBLimitsModel {
    offSetNumber: number;
    limitedNumber: number;
    filterSearchName: string;
    totalCount: number;
    

    constructor() {
        this.offSetNumber = 0;
        this.limitedNumber = 30;        
        this.filterSearchName = "";        
        this.totalCount = 0;        
    }
}

export class DBUsersListModel {
    id: string;
    userId: string;
    userName: string;
    userToken: string;
    epoc: string;
    dateTime: string;
    

    constructor() {
        this.id = "";
        this.userId = "";
        this.userName = "";
        this.userToken = "";
        this.epoc = "";
        this.dateTime = "";
    }
}