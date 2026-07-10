import { Category } from "./Category.model";
import { Donor } from "./Donor.model";
import { User } from "./User.model";

export class Gift{
    id!: number;
    name!: string;
    description! : string;
    price : number= 10; 
    categoryId! : number;
    donorId!: string;
    donor!: Donor;
    donorName!: string;
    imageUrl!: string;
    winnerUser!: User;
    isRandomized!: boolean;
    category!: Category;
    categoryName!: string;
}