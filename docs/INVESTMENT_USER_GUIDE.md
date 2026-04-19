# Investment Portfolio Tool - User Guide

## Overview

The Investment Portfolio Tool allows you to manage multiple investment portfolios tracking cryptocurrencies, ETFs, stocks, bonds, and other assets. Each portfolio tracks individual investments with their current values and gain/loss metrics.

## Getting Started

### 1. Create a Portfolio

1. Click the **Portfolios** button in the toolbar
2. Click **Create New Portfolio**
3. Enter:
   - **Portfolio Name**: e.g., "Retirement Fund", "Long-term", "Short-term Trading"
   - **Description** (optional): e.g., "401k and IRA accounts"
4. Click **Create Portfolio**

### 2. Add Investments

1. Click the **Investments** button in the toolbar
2. Select your portfolio from the dropdown
3. Click **Add Investment** (or the + button in Portfolio Management)
4. Fill in the investment details:
   - **Portfolio**: Select which portfolio this belongs to
   - **Asset Type**: Choose from Cryptocurrency, ETF, Stock, Bond, Gold, Deposit, Real Estate, Other
   - **Symbol/Name**: e.g., "BTC", "VTSAX", "AAPL", "Gold Bars"
   - **Quantity**: How many units you own
   - **Purchase Price**: What you paid per unit
   - **Purchase Date**: When you bought it
   - **Current Price**: Current value per unit (will update manually)
5. Click **Add Investment**

### 3. View Your Portfolio

1. Click **Investments** in the toolbar
2. Select your portfolio from the dropdown
3. View:
   - **Portfolio Summary**: Total cost, value, gain/loss, and percentage gain/loss
   - **Asset Allocation**: Breakdown of your portfolio by asset type
   - **Investments Table**: All individual holdings with:
     - Symbol and asset type
     - Quantity and purchase price
     - Current price and total value
     - Gain/loss in dollars and percentage

### 4. Update Investment Values

When market prices change:

1. Click **Investments** → select portfolio
2. Find the investment in the table
3. Click the delete button to remove it (or use update button if available in future versions)
4. Re-add it with the new current price

Alternatively:
- Click **Portfolios** → Edit → Update portfolio details
- Or wait for Phase 2 to implement automatic daily price updates from market APIs

### 5. Edit or Delete Portfolios

1. Click **Portfolios** in the toolbar
2. For each portfolio you can:
   - **View**: Click the eye icon to see investments
   - **Edit**: Click the pencil icon to update name/description
   - **Delete**: Click the trash icon to remove (⚠️ removes all investments in portfolio)

## Understanding the Metrics

### Total Cost
The sum of (Quantity × Purchase Price) for all investments

### Total Value
The sum of (Quantity × Current Price) for all investments

### Gain/Loss
Total Value - Total Cost
- **Positive (green)**: Your portfolio is up
- **Negative (red)**: Your portfolio is down

### Gain/Loss %
(Gain/Loss ÷ Total Cost) × 100
- Percentage return on your investment

### Asset Allocation
Breakdown of portfolio value by asset type
- Helps understand diversification
- Shows what percentage of your portfolio each asset type represents

## Tips

1. **Multiple Portfolios**: Create separate portfolios for different goals:
   - Retirement accounts
   - Emergency fund
   - Taxable trading
   - Crypto/speculative

2. **Regular Updates**: Update current prices regularly (daily/weekly) to track performance

3. **Asset Naming**: Use clear symbols:
   - BTC, ETH (crypto)
   - VTSAX, VOO (ETFs)
   - AAPL, GOOGL (stocks)
   - "Gold Bars", "Emergency Fund" (deposits)

4. **Allocation**: Aim for balanced allocation across asset types aligned with your goals

## Data Privacy

- All portfolio data is stored securely in MongoDB
- Only accessible to your authenticated account
- Cannot see other users' portfolios

## Future Enhancements (Phase 2+)

- ⏳ Automatic daily price updates from market APIs
- ⏳ Historical price tracking and trend charts
- ⏳ Portfolio performance comparison
- ⏳ Year-over-year returns
- ⏳ Tax cost basis reporting
- ⏳ Rebalancing recommendations
- ⏳ Mobile app support

## Troubleshooting

**Q: My portfolio doesn't show investments**
A: Make sure you've added investments to your portfolio. Create one first, then add investments.

**Q: Current prices not updating automatically**
A: Market price updates are coming in Phase 2. For now, manually update prices by deleting and re-adding investments.

**Q: Can't find a portfolio**
A: Portfolios are scoped to your Google account. Make sure you're logged in with the correct account.

**Q: Edit button not working**
A: Use Portfolios view → Edit → Update name/description. Investment editing coming in Phase 4.
