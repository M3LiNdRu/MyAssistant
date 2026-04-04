export interface CurrentSummary {
    month: string;
    year: string;
    totalAmount: number;
    progressBar: Dictionary<number>
}

interface Dictionary<T> {
    [Key: string]: T;
}