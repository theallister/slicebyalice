export interface CartItem {
    productId: number
    quantity: number
    productType?: string
    imageUrl?: string
    name?: string
    price?: number
    twoForPrice?: number
    totalPrice?: number
    url?: string
    inStock?: number
    selectedEnvelopes?: string[]
}