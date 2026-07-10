import { Gift } from "./Gift.model";

export class Donor{
    id!: string;
    name!: string;
    email! : string;
    gifts?: Gift[];
}