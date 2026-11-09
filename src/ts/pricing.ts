import { CartItem } from "./objects"

export const lineTotal = ({ price = 0, twoForPrice, quantity }: CartItem): number => {
    if (!twoForPrice) return price * quantity
    const pairs = Math.floor(quantity / 2)
    return pairs * twoForPrice + (quantity % 2) * price
}

export const cartTotal = (items: CartItem[]): number => {
    let total = 0
    const deals = new Map<number, number[]>()

    for (const { price = 0, twoForPrice, quantity } of items) {
        if (!twoForPrice) {
            total += price * quantity
            continue
        }
        const units = deals.get(twoForPrice) ?? []
        for (let i = 0; i < quantity; i++) units.push(price)
        deals.set(twoForPrice, units)
    }

    for (const [twoForPrice, units] of deals) {
        total += Math.floor(units.length / 2) * twoForPrice
        if (units.length % 2 === 1) total += Math.min(...units)
    }
    return total
}