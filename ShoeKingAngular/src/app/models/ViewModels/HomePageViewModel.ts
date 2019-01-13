import { DecimalPipe } from "@angular/common";


export class HomePageViewModel { 
    Id: number;
    TextHeader: string;
    Text: string;
    ImageUrl: string;
}

export class HomeNewArrivalsViewModel {
    Id: number;
    Name: string;
    BgName: string;
    Price: number;
    SizeOfProduct: SizeOfHomeProduct;
    Status: boolean;
    Photo: string;
    Specials: boolean;
    PromotionPercent: number;
    PromotionPrice: number;
    Description: string;
    BgDescription: string;
    ProductId: number;
}

export class SizeOfHomeProduct {
    SizeId: number;
    Size: number;
}