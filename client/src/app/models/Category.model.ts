import { Gift } from "./Gift.model";

export class Category{
    id!: number;
    name!: string;
    gifts!: Gift[];
}