// Converts all decimal fields in Transactions documents to NumberDecimal (Decimal128):
//   - totalAmount.amount
//   - fees[].fee.amount
//   - stock.price.amount
//   - stock.quantity
//
// Run from within the mongo-tools container, see ../USAGE.md:
// mongosh mongodb://mongo:27017/<db>?directConnection=true /scripts/convert-transaction-amounts-to-decimal.js

const collection = db.getCollection("Transactions");

const cursor = collection.find({
  $or: [
    { "totalAmount.amount": { $type: "string" } },
    { "fees.fee.amount": { $type: "string" } },
    { "stock.price.amount": { $type: "string" } },
    { "stock.quantity": { $type: "string" } },
  ],
});

let updatedCount = 0;

cursor.forEach((transaction) => {
  const update = {};

  if (typeof transaction.totalAmount?.amount === "string") {
    update["totalAmount.amount"] = NumberDecimal(transaction.totalAmount.amount);
  }

  if (Array.isArray(transaction.fees)) {
    transaction.fees.forEach((fee, index) => {
      if (typeof fee.fee?.amount === "string") {
        update[`fees.${index}.fee.amount`] = NumberDecimal(fee.fee.amount);
      }
    });
  }

  if (typeof transaction.stock?.price?.amount === "string") {
    update["stock.price.amount"] = NumberDecimal(transaction.stock.price.amount);
  }

  if (typeof transaction.stock?.quantity === "string") {
    update["stock.quantity"] = NumberDecimal(transaction.stock.quantity);
  }

  if (Object.keys(update).length > 0) {
    collection.updateOne({ _id: transaction._id }, { $set: update });
    updatedCount++;
  }
});

print(`Updated ${updatedCount} transaction(s).`);
