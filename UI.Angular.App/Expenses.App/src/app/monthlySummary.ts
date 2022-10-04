export interface MonthlySummary {
    month: string;
    year: string;
    start: number;
    saved: number;
    progressBar: Dictionary<number>;
    spentByCategory: Dictionary<number>
}

export interface Dictionary<T> {
    [Key: string]: T;
}