
$(function () {
    'use strict';

    var startBtn = $('#tour');


    function setupTour(tour, listElement) {
        var backBtnClass = 'btn btn-sm btn-outline-primary',
            nextBtnClass = 'btn btn-sm btn-outline-primary',
            doneBtnClass = 'btn btn-sm btn-primary btn-next';

        let index = 0;
        listElement.map(element => {
            index++;

            let buttonConfig = [
                {
                    action: tour.cancel,
                    classes: backBtnClass,
                    text: 'Bỏ qua'
                },
                {
                    text: 'Tiếp theo',
                    classes: nextBtnClass,
                    action: tour.next
                }
            ]

            if (index > 1 && index < listElement.length) {
                buttonConfig = [
                    {
                        text: 'Quay lại',
                        classes: backBtnClass,
                        action: tour.back
                    },
                    {
                        text: 'Tiếp theo',
                        classes: nextBtnClass,
                        action: tour.next
                    }
                ]
            }
            else if (index == listElement.length) {
                buttonConfig = [
                    {
                        action: tour.cancel,
                        classes: backBtnClass,
                        text: 'Bỏ qua'
                    },
                    {
                        text: 'Hoàn thành',
                        classes: doneBtnClass,
                        action: tour.cancel
                    }
                ]
            }

            tour.addStep({
                title: element.title,
                text: element.content,
                attachTo: { element: element.name, on: element.position },
                beforeShowPromise: () => {
                    return new Promise((resolve) => {
                        // Scroll to the target element and center it on the screen
                        const elementScrl = document.querySelector(element.name);
                        const elementRect = elementScrl.getBoundingClientRect();
                        const scrollPosition = elementRect.top + window.scrollY - (window.innerHeight / 2);

                        window.scrollTo({ top: scrollPosition, behavior: 'smooth' });
                        setTimeout(resolve, 200); // Adjust the delay as needed to ensure scrolling completes before the step is shown
                    });
                },
                buttons: buttonConfig
            });


            // Attach an event handler to the "finish" event
            tour.on('cancel', () => {
                // Scroll to the top of the page
                window.scrollTo({ top: 0, behavior: 'smooth' });
            });
        })
        
        return tour;
    }

    function startTourFrontAndBack(listElement) {
        var tourVar = new Shepherd.Tour({
            defaultStepOptions: {
                classes: 'shadow-md bg-purple-dark',
                scrollTo: false,
                cancelIcon: {
                    enabled: true
                }
            },
            useModalOverlay: true
        });
        setupTour(tourVar, listElement).start();
    }

    window.startTourFrontAndBack = startTourFrontAndBack;
});