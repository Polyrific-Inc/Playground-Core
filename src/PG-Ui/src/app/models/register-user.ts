export class RegisterUser {
    /**
     *
     */
    constructor(
        public email: string,
        public firstName: string,
        public lastName: string,
        public password: string,
        public confirmPassword: string
    ) { }    
}