appAngularjs.controller('sinhvienController', function ($scope) {
    $scope.sinhvien = {
        ho: 'Nguyễn Đức',
        ten: 'Tài',
        hocphi: '2000000',
        tenmonhoc: [
            { ten: 'TOÁN', diemthi: 9 },
            { ten: 'LÝ', diemthi: 9.5 },
            { ten: 'HÓA', diemthi: 7 },
            { ten: 'ĐỊA', diemthi: 8 }
        ],
        hoten: function () {
            var doituongsinhvien;
            doituongsinhvien = $scope.sinhvien;
            return doituongsinhvien.ho + ' ' + doituongsinhvien.ten;
        }
    };
});