import Swiper from 'swiper'
import { Navigation, Pagination, A11y } from 'swiper/modules'

const swiperContainers = document.querySelectorAll('.swiper') as NodeListOf<HTMLElement>

let swiperA11y = {
    nextSlideMessage: 'Nästa bild',
    prevSlideMessage: 'Föregående bild',
    firstSlideMessage: 'Detta är den första bilden',
    lastSlideMessage: 'Detta är den sista bilden',
    paginationBulletMessage: 'Gå till bild {{index}}',
    slideLabelMessage: 'Bild {{index}} av {{slidesLength}}',
    containerMessage: 'Bildspel',
}
export function initSwipers() {
    swiperContainers.forEach(container => {
        const swiperType = container.dataset.swiperType

        switch (swiperType) {
            case 'products':
                swiperA11y.containerMessage = 'Bildspel med produkter'
                break;
            default:
                break;
        }
        
        const swiper = new Swiper(container, {
            modules: [Navigation, Pagination, A11y],
            spaceBetween: 16,
            slidesPerView: 1.25,
            slidesOffsetBefore: 16,
            slidesOffsetAfter: 16,
            navigation: {
                nextEl: container.querySelector('.swiper-button-next'),
                prevEl: container.querySelector('.swiper-button-prev'),
                addIcons: false
            },
            pagination: {
                el: container.querySelector('.swiper-pagination'),
                clickable: true,
                renderBullet: (index, className) => {
                    return `
                        <button type="button" class="${className}" aria-label="Gå till bild ${index + 1}">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 113.8 101.26">
                                <path fill="none" stroke="#221f20" stroke-linecap="round" stroke-linejoin="round" stroke-width="16px" d="M14.17,32.09c-8.12,13.09-8.53,31.1.55,43.55,10.74,14.72,31.16,18.72,49.33,17.39,14.42-1.06,30.08-5.75,37.6-18.1,8.47-13.92,3.06-32.89-8.12-44.74-9.15-9.69-27.8-22.85-41.66-22.15-13.94.7-30.6,12.62-37.7,24.06Z"/>
                            </svg>
                        </button>
                    `
                },
            },
            a11y: swiperA11y,
            breakpoints: {
                480: {
                    slidesPerView: 2.25,
                    spaceBetween: 24,
                    slidesOffsetBefore: 24,
                    slidesOffsetAfter: 24,
                },
                720: {
                    slidesOffsetBefore: 8,
                    slidesOffsetAfter: 8,
                    slidesPerView: 2.75,
                    spaceBetween: 24,
                },
                1113: {
                    slidesOffsetBefore: 8,
                    slidesOffsetAfter: 8,
                    slidesPerView: 3,
                    spaceBetween: 24,
                },
                1440: {
                    slidesOffsetBefore: 8,
                    slidesOffsetAfter: 8,
                    slidesPerView: 4, 
                    spaceBetween: 24,
                }
            }
        })

        swiper.init()
    })
}