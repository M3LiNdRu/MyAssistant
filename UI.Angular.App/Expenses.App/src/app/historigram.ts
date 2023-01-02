export interface Historigram {
    totalSavings: number;
    totalEarned: number;
    totalSpent: number;
    progressLine: Dictionary<HistorigramDot>
}

interface Dictionary<T> {
    [Key: string]: T;
}

export interface HistorigramDot {
    saved: number;
    spent: number;
    earned: number;
}