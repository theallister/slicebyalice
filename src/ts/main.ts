import { Fancybox } from "@fancyapps/ui/dist/fancybox/";

import Alpine from 'alpinejs'
import focus from '@alpinejs/focus'
import { initSwipers } from "./swipers"
 
Alpine.plugin(focus)

import { CartItem } from "./objects"
import { lineTotal, cartTotal } from "./pricing"

Alpine.data('storeFilter', () => ({
    open: false,
    selectedCategory: 'all',
    selectedTags: [] as string[],
        showHideItem(category: string, tag: string): boolean {
        const matchesCategory = this.selectedCategory === 'all' || this.selectedCategory === category
        if (!matchesCategory) {
            return false
        }
        if (this.selectedTags.length === 0) {
            return true
        }
        const itemTags = new Set(tag.split(',').map(t => t.trim()))
        return this.selectedTags.some(selectedTag => itemTags.has(selectedTag))
    }
}))

const cartStore = {
    availableEnvelopes: [] as string[],
        items: [] as CartItem[],

    init() {
        this.items = JSON.parse(localStorage.getItem('cart') || '[]')
    },
    get itemsInCart() {
        return this.items.reduce((n, i) => n + i.quantity, 0)
    },
    get totalPrice() {
        return cartTotal(this.items)
    },
    get fullPrice() {
        return this.items.reduce((s, i) => s + (i.price ?? 0) * i.quantity, 0)
    },
    get savings() {
        return this.fullPrice - this.totalPrice
    },
    save() {
        localStorage.setItem('cart', JSON.stringify(this.items))
    },
    async addToCart(productId: number, quantity: number) {
        const existing = this.items.find(i => i.productId === productId)
        if (existing) {
            existing.quantity += quantity
        } else {
            const p = await fetch(`/api/product/item/${productId}`).then(r => r.json())
            this.items.push({
                productId,
                quantity,
                productType: p.productType,
                imageUrl: p.imageUrl,
                name: p.name,
                price: p.price,
                twoForPrice: p.twoForPrice,
                url: p.url,
                inStock: p.inStock,
            })
        }
        this.save()
    },
    updateAmountInCart(productId: number, quantity: number) {
        if (quantity <= 0) return this.removeFromCart(productId)
        const item = this.items.find(i => i.productId === productId)
        if (item) item.quantity = quantity
        this.save()
    },
    removeFromCart(productId: number) {
        this.items = this.items.filter(i => i.productId !== productId)
        this.save()
    },
    addEnvelopeToCart(productId: number, envelope: string, amount: number) {
        const cart: CartItem[] = JSON.parse(localStorage.getItem('cart') || '[]')
        const item = cart.find(item => item.productId === productId)
        if (item) {
            item.selectedEnvelopes ??= [];
            for (let i = 0; i < amount; i++) {
                item.selectedEnvelopes.push(envelope)
            }
        }
        this.save()
    },
    removeEnvelopeFromCart(productId: number, envelope: string, amount: number) {
        const cart: CartItem[] = JSON.parse(localStorage.getItem('cart') || '[]')
        const item = cart.find(item => item.productId === productId)
        if (item && item.selectedEnvelopes) {
            for (let i = 0; i < amount; i++) {
                const index = item.selectedEnvelopes.indexOf(envelope)
                if (index !== -1) {
                    item.selectedEnvelopes.splice(index, 1)
                }
            }
        }
        this.save()
    }
}

Alpine.store('cart', cartStore)

const calculateProductPrice = (price?: number, twoForPrice?: number, quantity: number = 1): number => {
    if (!price) return 0
    if (twoForPrice && quantity >= 2) {
        const twoForCount = Math.floor(quantity / 2)
        const remainder = quantity % 2
        return twoForCount * twoForPrice + remainder * price
    }
    return price * quantity
}

Alpine.data('store', () => ({
    windowScrolled: false,
    navOpen: false,
    cartOpen: false,
    searchOpen: false,
    closeAll() {
        this.navOpen = false
        this.cartOpen = false
        this.searchOpen = false
    }
}))

interface FormItem extends HTMLElement {
    name?: string
    value: string
    invalid: string
    ariaInvalid: string
}
Alpine.data('form', () => ({
    errors: {
        firstName: false,
        lastName: false,
        email: false,
        phone: false,
        address: false,
        postalCode: false,
        city: false
    },
    updateErrors(el: FormItem) {
        this.errors[el.name as keyof typeof this.errors] = el.classList.contains('invalid')
    },
    validate(el: FormItem) {
        const value = el.value
        if (value.trim() === '') {
            el.invalid = 'true'
            el.ariaInvalid = 'true'
            el.classList.add('invalid')
        } else {
            el.invalid = 'false'
            el.ariaInvalid = 'false'
            el.classList.remove('invalid')
        }
    }
}))

Alpine.start()

Fancybox.bind("[data-fancybox]", {})

initSwipers()